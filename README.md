# OptimizedScrollView

OptimizedScrollViewは、Unityでデータソースを効率的にスクロールビューで表示するためのライブラリです。大量のデータを扱う場合でも、実際に画面に表示されている部分のみを描画することで、パフォーマンスを最適化します。

## 特徴

- 表示されている要素のみをレンダリングする仮想スクロールの実装
- 様々なデータソースに対応可能な柔軟なインターフェース
- カスタマイズ可能なセルレイアウト
- 簡単に導入できるシンプルなAPI

## インストール方法

1. このリポジトリをクローンまたはダウンロードします
2. `Assets/OptimizedScrollView` フォルダをUnityプロジェクトにインポートします

## 基本的な使い方

### 1. ScrollViewコンポーネントの設定

Hierarchyビューで新しいGameObjectを作成し、`OptimizedScrollView`コンポーネントをアタッチします。

```csharp
// ScrollViewの基本設定例
OptimizedScrollView scrollView = gameObject.AddComponent<OptimizedScrollView>();
scrollView.Initialize();
```

### 2. データソースの実装

`IDataSource` インターフェースを実装したクラスを作成します。

```csharp
public class MyDataSource : IDataSource
{
    private List<string> _items = new List<string>();
    
    public int Count => _items.Count;
    
    public void GetItemAt(int index, IScrollViewCell cell)
    {
        MyCell myCell = cell as MyCell;
        if (myCell != null)
        {
            myCell.SetData(_items[index]);
        }
    }
    
    public float GetCellHeight(int index)
    {
        return 100f; // 固定高さの例
    }
}
```

### 3. セルの実装

`IScrollViewCell` インターフェースを実装したセルクラスを作成します。

```csharp
public class MyCell : MonoBehaviour, IScrollViewCell
{
    [SerializeField] private Text _label;
    
    public void SetData(string data)
    {
        _label.text = data;
    }
    
    public void OnRecycle()
    {
        // セルが再利用される際のクリーンアップ処理
        _label.text = string.Empty;
    }
}
```

### 4. ScrollViewの初期化と使用

```csharp
// データソースとセルのプレハブを設定
scrollView.SetDataSource(myDataSource);
scrollView.SetCellPrefab(myCellPrefab);
scrollView.ReloadData();
```

## サンプルの解説

このリポジトリには、OptimizedScrollViewの使用方法を示すいくつかのサンプルが含まれています。

### シンプルリストサンプル

`/Samples/SimpleList` フォルダにあるサンプルは、テキストデータを表示する基本的な使用方法を示しています。

```csharp
// SimpleListDataSource.cs の主要部分
public class SimpleListDataSource : IDataSource
{
    private List<string> _items;
    
    public SimpleListDataSource(int count)
    {
        _items = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            _items.Add($"Item {i}");
        }
    }
    
    public int Count => _items.Count;
    
    public void GetItemAt(int index, IScrollViewCell cell)
    {
        SimpleListCell listCell = cell as SimpleListCell;
        listCell?.SetData(_items[index]);
    }
    
    public float GetCellHeight(int index)
    {
        return 50f;
    }
}
```

### 可変高さセルサンプル

`/Samples/VariableHeight` フォルダのサンプルは、セルごとに異なる高さを持つリストの表示方法を示しています。

```csharp
// VariableHeightDataSource.cs の主要部分
public class VariableHeightDataSource : IDataSource
{
    private List<ItemData> _items;
    
    public VariableHeightDataSource(int count)
    {
        _items = new List<ItemData>(count);
        System.Random random = new System.Random();
        
        for (int i = 0; i < count; i++)
        {
            // ランダムなテキスト長とそれに応じた高さを設定
            int contentLength = random.Next(1, 5);
            string content = string.Join("\n", Enumerable.Repeat($"Content line for item {i}", contentLength));
            
            _items.Add(new ItemData
            {
                Title = $"Item {i}",
                Content = content
            });
        }
    }
    
    public int Count => _items.Count;
    
    public void GetItemAt(int index, IScrollViewCell cell)
    {
        VariableHeightCell varCell = cell as VariableHeightCell;
        varCell?.SetData(_items[index]);
    }
    
    public float GetCellHeight(int index)
    {
        // コンテンツの量に基づいて高さを計算
        int lineCount = _items[index].Content.Split('\n').Length;
        return 60f + (lineCount * 20f);
    }
}
```

### 画像リストサンプル

`/Samples/ImageList` フォルダのサンプルは、テキストと画像を含むリストの表示方法を示しています。

```csharp
// ImageListCell.cs の主要部分
public class ImageListCell : MonoBehaviour, IScrollViewCell
{
    [SerializeField] private Text _titleText;
    [SerializeField] private Image _thumbnail;
    
    public void SetData(ImageItemData data)
    {
        _titleText.text = data.Title;
        _thumbnail.sprite = data.Thumbnail;
    }
    
    public void OnRecycle()
    {
        _titleText.text = string.Empty;
        _thumbnail.sprite = null;
    }
}
```

## パフォーマンス最適化のヒント

- セルの再利用を適切に処理するために `OnRecycle()` メソッドを必ず実装してください
- 大きなデータセットでは、ページング処理を実装することも検討してください
- 画像やアニメーションなど重いコンテンツを含むセルでは、遅延ロードを実装することをお勧めします

## ライセンス

MITライセンスの下で公開されています。詳細については、LICENSE ファイルを参照してください。