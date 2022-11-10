using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interaction with inventory Grid and creating it
/// </summary>
public class InventoryGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;

    RectTransform rectTransform;
    Vector2 positionOnTheGrid;
    Vector2Int tileGridPosition;

    ItemFromInventory[,] inventoryItemsSlot;

    [SerializeField]
    Vector2Int gridSize;

    [SerializeField]
    GameObject inventoryItemPrefab;

    public Vector2 PositionOnTheGrid { get => positionOnTheGrid; set => positionOnTheGrid = value; }
    public Vector2Int TileGridPosition { get => tileGridPosition; set => tileGridPosition = value; }
    public Vector2Int GridSize { get => gridSize; set => gridSize = value; }
    public ItemFromInventory[,] InventoryItemsSlot { get => inventoryItemsSlot; set => inventoryItemsSlot = value; }
    public RectTransform GridRectTransform { get => rectTransform; set => rectTransform = value; }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init();

        positionOnTheGrid = new Vector2();
        tileGridPosition = new Vector2Int();
    }

    private void Init(int width, int height)
    {
        inventoryItemsSlot = new ItemFromInventory[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;

        //Testing
        //InventoryItem inventoryItem = Instantiate(inventoryItemPrefab.GetComponent<InventoryItem>());
        //PlaceItem(inventoryItem, 3, 2);
    }

    internal ItemFromInventory GetItem(Vector2Int positionOnGrid)
    {
        Debug.Log("GetItem from " + positionOnGrid);
        return inventoryItemsSlot[positionOnGrid.x, -(positionOnGrid.y)];
    }

    private void Init()
    {
        inventoryItemsSlot = new ItemFromInventory[gridSize.x, gridSize.y];
        Vector2 size = new Vector2(gridSize.x * tileSizeWidth, gridSize.y * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetInvGridPositon(Vector2 mousePosition)
    {

        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = mousePosition.y - rectTransform.position.y;

        tileGridPosition.x = (int) (positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int) (positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public bool PlaceItem(ItemFromInventory inventoryItem, int posX, int posY, ref ItemFromInventory overlapItem)
    {
        if (!BoundryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height))
        {
            return false;
        }

        if (!OverlapCheck(posX, posY, inventoryItem.itemData, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItemToGrid(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItemToGrid(ItemFromInventory inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        Debug.Log($"size {inventoryItem.itemData.fill.Length}");

        for (int i = 0; i < inventoryItem.itemData.width; i++)
        {
            for (int j = 0; j < inventoryItem.itemData.height; j++)
            {

                if (inventoryItem.itemData.fill[i, j])
                {
                    Debug.Log($"Place item in x{posX + i} y{posY + j}");
                    inventoryItemsSlot[posX + i, posY + j] = inventoryItem;
                    
                }

            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        //rectTransform.parent = rectTransform;
        //inventoryItemsSlot[posX, posY] = inventoryItem;
        Vector2 position = GetItemPosition(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    public Vector2 GetItemPosition(ItemFromInventory itemFromInventory, int posX, int posY)
    {
        Vector2 position = new Vector2();
        //position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.itemData.width / 2;
        //position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.itemData.height / 2);

        position.x = posX * tileSizeWidth;
        position.y = -(posY * tileSizeHeight);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, ItemData holdedItem, ref ItemFromInventory overlapItem)
    {
        for(int x = 0; x < holdedItem.width; x++)
        {
            for(var y = 0; y < holdedItem.height; y++)
            {
                try
                {
                    if(holdedItem.fill[x,y])
                    {
                        if (inventoryItemsSlot[posX + x, posY + y] != null)
                        {
                            if (overlapItem == null)
                            {
                                overlapItem = inventoryItemsSlot[posX + x, posY + y];
                            }
                            else
                            {
                                if (overlapItem != inventoryItemsSlot[posX + x, posY + y])
                                {
                                    return false;
                                }

                            }

                        }
                    }
                    
                }
                catch (IndexOutOfRangeException indexOUT)
                {
                    Debug.Log("Checking outside of grid, item not match " + indexOUT);
                    return false;
                }
                
            }
        }

        return true;
    }

    private bool CheckAvailableSpace(int posX, int posY, ItemData holdedItem)
    {
        for (int x = 0; x < holdedItem.width; x++)
        {
            for (var y = 0; y < holdedItem.height; y++)
            {
                try
                {
                    if (holdedItem.fill[x, y])
                    {
                        if (inventoryItemsSlot[posX + x, posY + y] != null)
                        {
                            return false;
                        }
                    }

                }
                catch (IndexOutOfRangeException indexOUT)
                {
                    Debug.Log("Checking outside of grid, item not match " + indexOUT);
                    return false;
                }

            }
        }

        return true;
    }

    public ItemFromInventory PickUpItem(int x, int y)
    {
        ItemFromInventory toReturn = inventoryItemsSlot[x, y];

        if (toReturn == null) { return null; }

        //inventoryItemsSlot[x, y] = null;

        CleanGridReference(toReturn);

        return toReturn;
    }

    private void CleanGridReference(ItemFromInventory item)
    {
        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                if (item.itemData.fill[i, j])
                {
                    inventoryItemsSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
                }

            }
        }
    }

    internal Vector2Int? FindSpaceForObject(ItemFromInventory itemToInsert)
    {
        for (int j = 0; j < gridSize.y; j++)
        {
            for(int i = 0; i < gridSize.x; i++)
            {
                if(CheckAvailableSpace(i, j, itemToInsert.itemData))
                {
                    return new Vector2Int(i,j);
                }
            }
        }

        return null;
    }

    internal bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY > 0)
        {
            return false;
        }

        if (posX >= gridSize.x || posY <= -gridSize.y)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Check if item is fully in an inventory
    /// </summary>
    /// <param name="posX">range from 0 to gridSize.x</param>
    /// <param name="posY"> range from 0 to -gridSize.y</param>
    /// <param name="width">Item parameter</param>
    /// <param name="height">Item parameter</param>
    /// <returns></returns>
    internal bool BoundryCheck(int posX, int posY, int width, int height)
    {
        posY = -posY;
        if(!PositionCheck(posX, posY)) return false;

        posX += width-1;
        posY -= height-1;
        //posY += height-1;

        if (!PositionCheck(posX, posY)) return false;

        return true;
    }

    
}
