using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighterLoot;
    [SerializeField] RectTransform highlighterTrade;
    [SerializeField] RectTransform highlighterPlayer;
    RectTransform highlighter;

    private void Start()
    {
        highlighter = highlighterLoot;
    }
    private void Update()
    {
        if(GameManager.Instance.canvasType == CANVAS_TYPE.Loot)
        {
            highlighter = highlighterLoot;
        }
        if(GameManager.Instance.canvasType == CANVAS_TYPE.Trade)
        {
            highlighter = highlighterTrade;
        }
        if(GameManager.Instance.canvasType == CANVAS_TYPE.Player)
        {
            highlighter = highlighterPlayer;
        }
    }

    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
        highlighter.SetAsLastSibling();
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth;
        size.y = targetItem.HEIGHT * ItemGrid.tileSizeHeight;
        highlighter.sizeDelta = size;
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        SetParent(targetGrid);

        // Calculate position on the Grid and assign the ItemGrid place -> Item
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);

        highlighter.localPosition = pos;

    }

    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null) { return; }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    // overload
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(targetItem, posX, posY);

        highlighter.localPosition = pos;

    }
    
}
