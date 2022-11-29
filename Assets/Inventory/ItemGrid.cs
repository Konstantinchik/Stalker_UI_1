using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All methods for interaction with Grid
/// </summary>
public class ItemGrid : MonoBehaviour
{
    // get mouse position and convert to Tile position
    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;

    // 
    InventoryItem[,] inventoryItemSlot;

    // Grid
    RectTransform rectTransform;


    [SerializeField] int gridSizeWidth;
    [SerializeField] int gridSizeHeight;

    [SerializeField] GameObject inventoryItemPrefab;

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // set Grid size
        Init(gridSizeWidth, gridSizeHeight);

        /*
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 1, 1);*/
        /*
        inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 3, 3);

        inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 5, 5);
        */
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        // store the reference to the Item to be returned into th variable
        // nullify the position
        // return Item
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null)
        {
            return null;
        }

        // remove big Item from the Grid
        CleanGridReference(toReturn);

        //inventoryItemSlot[x, y] = null;
        return toReturn;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        // return position Item from Array
        return inventoryItemSlot[x, y];
    }

    internal Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.HEIGHT + 1;
        int width = gridSizeWidth - itemToInsert.WIDTH + 1;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.itemData.width, itemToInsert.itemData.height) == true)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    // return position of the Tile on the Grid
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

   
    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        // place item on the Grid
        // first - boundry check
        if (BoundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == false)
        {
            // JUST DONT PLACE
            return false;
        }

        #region OVERLAP
        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }
        #endregion

        // for change position of the objrct in the Canvas
        PlaceItem(inventoryItem, posX, posY);

        // if we can place Item
        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        // viewig specific Item to specific Tile on the Grid
        for (int x = 0; x < inventoryItem.WIDTH; x++)
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        // Assign onGridPosition - big objects
        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;


        // calculate position Item on the screen
        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    // -- Returt the Item
    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        // Assign overlap to the Item
        for(int x=0; x < width; x++)
        {
            for(int y=0; y< height; y++)
            {
                if(inventoryItemSlot[posX+x, posY+y]!= null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if (overlapItem != inventoryItemSlot[posX + y, posY + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }


    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        return position;
    }

    

    // Calculation for auto-insert available space
    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Prevent out of bound
    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
            return false;

        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
            return false;

        return true;
    }

    // Check boudrys for object which will ocupie multiple-grid tiles
    public bool BoundryCheck(int posX, int posY, int width, int height)
    {
        // reuse position check - origin position
        // check top left position of the object
        if (PositionCheck(posX, posY) == false)
            return false;

        // Add width and height to the positon
        posX += width - 1;
        posY += height - 1;

        // check bottom right position of this item
        if (PositionCheck(posX, posY) == false)
            return false;

        return true;
    }
}
