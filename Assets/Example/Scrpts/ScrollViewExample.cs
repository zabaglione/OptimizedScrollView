using System; // 追加: Action デリゲートを使用するために必要
using UnityEngine;
using UnityEngine.UI;
using Zabaglione.OptimizedScrollView;

/// <summary>
/// ScrollViewの使用例クラス
/// </summary>
public class ScrollViewExample : MonoBehaviour
{
    [Header("ScrollView設定")]
    [SerializeField] private OptimizedScrollViewView scrollViewView; // Inspectorで設定

    private OptimizedScrollViewModel<string> model;
    private OptimizedScrollViewPresenter<string> presenter;

    private int itemCount = 0;

    private  System.Random random = new System.Random();

    private void Awake()
    {
        // 必要なコンポーネントが設定されているか確認
        if(scrollViewView == null)
        {
            Debug.LogError("OptimizedScrollViewViewがScrollViewExampleに設定されていません。");
        }

        // ModelとPresenterの初期化
        model = new OptimizedScrollViewModel<string>();

        // ユーザー提供のアイテム更新コールバックを定義
        Action<int, GameObject, string> updateItemCallback = (index, item, data) =>
        {
            // ユーザー側で独自にアイテムを設定するロジックを記述
            // 例として、Textコンポーネントが存在する場合は設定する
            var text = item.GetComponentInChildren<Text>();
            if(text != null)
            {
                text.text = $"#{index + 1}: {data}";
            }
            else
            {
                Debug.LogWarning("ScrollViewExample: アイテムにTextコンポーネントが見つかりません。");
            }

            // 追加のUI要素がある場合はここで設定
            // 例:
            // var image = item.GetComponentInChildren<Image>();
            // if(image != null)
            // {
            //     image.sprite = someSprite;
            // }
        };

        presenter = new OptimizedScrollViewPresenter<string>(model, scrollViewView, updateItemCallback);
    }

    private void Update()
    {
        // Aキーでアイテムを追加
        if(Input.GetKeyDown(KeyCode.A))
        {
            AddItem();
        }

        // Dキーでアイテムを削除
        if(Input.GetKeyDown(KeyCode.D))
        {
            RemoveItem();
        }

        // Cキーで全アイテムをクリア
        if(Input.GetKeyDown(KeyCode.C))
        {
            ClearItems();
        }
    }

    /// <summary>
    /// アイテムを追加するメソッド
    /// </summary>
    private void AddItem()
    {
        itemCount++;
        string newItem = "Item " + itemCount;
        presenter.AddItem(newItem);
        Debug.Log($"アイテムを追加しました: {newItem}");
    }

    /// <summary>
    /// アイテムを削除するメソッド
    /// </summary>
    private void RemoveItem()
    {
        if(model.GetItemCount() > 0)
        {
            string lastItem = model.GetItem(random.Next(model.GetItemCount()));
            presenter.RemoveItem(lastItem);
            Debug.Log($"アイテムを削除しました: {lastItem}");
            itemCount--;
        }
        else
        {
            Debug.LogWarning("削除するアイテムがありません。");
        }
    }

    /// <summary>
    /// 全アイテムをクリアするメソッド
    /// </summary>
    private void ClearItems()
    {
        presenter.ClearItems();
        itemCount = 0;
        Debug.Log("全てのアイテムをクリアしました。");
    }
}
