using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Describe item in inventory
/// </summary>
public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int HEIGHT
    {
        get
        {
            if (rotated==false)
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }

    public int WIDTH
    {
        get
        {
            if (rotated == false)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    // origin Item position
    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;

    internal void Set(ItemData itemData)
    {
        // setup GameObject on the canvas
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        // set Item size
        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.tileSizeWidth;
        size.y = itemData.height * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    internal void Rotate()
    {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0);
    }
}
