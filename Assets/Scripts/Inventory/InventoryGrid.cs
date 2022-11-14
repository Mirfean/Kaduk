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
        if (!BoundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT))
        {
            return false;
        }

        if (!OverlapCheck(posX, posY, inventoryItem, ref overlapItem))
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

        for (int i = 0; i < inventoryItem.WIDTH; i++)
        {
            for (int j = 0; j < inventoryItem.HEIGHT; j++)
            {

                if (inventoryItem.spaceFill[i, j])
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

    /// <summary>
    /// Getting position of item on grid
    /// </summary>
    /// <param name="itemFromInventory"></param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public Vector2 GetItemPosition(ItemFromInventory itemFromInventory, int posX, int posY)
    {
        Vector2 position = new Vector2();

        position.x = posX * tileSizeWidth;
        position.y = -(posY * tileSizeHeight);
        return position;
    }

    /// <summary>
    /// Check if selected item is currently above another item
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="holdedItem"></param>
    /// <param name="overlapItem"></param>
    /// <returns></returns>
    private bool OverlapCheck(int posX, int posY, ItemFromInventory holdedItem, ref ItemFromInventory overlapItem)
    {
        for(int x = 0; x < holdedItem.WIDTH; x++)
        {
            for(var y = 0; y < holdedItem.HEIGHT; y++)
            {
                if (holdedItem.spaceFill[x, y])
                {
                    try
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
                    catch (IndexOutOfRangeException indexOUT)
                    {
                        Debug.Log("Checking outside of grid, item not match " + indexOUT);
                        return false;
                    }
                }
                
                
            }
        }

        return true;
    }

    /// <summary>
    /// Check if selected item can be put on certain space(checking all of item's spaces)
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="holdedItem"></param>
    /// <returns></returns>
    private bool CheckAvailableSpace(int posX, int posY, ItemFromInventory holdedItem)
    {
        for (int x = 0; x < holdedItem.WIDTH; x++)
        {
            for (var y = 0; y < holdedItem.HEIGHT; y++)
            {
                try
                {
                    if (holdedItem.spaceFill[x, y])
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

    /// <summary>
    /// Pick item and remove it's fill from inventoryGrid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public ItemFromInventory PickUpItem(int x, int y)
    {
        ItemFromInventory toReturn = inventoryItemsSlot[x, y];

        if (toReturn == null) { return null; }

        CleanGridReference(toReturn);

        return toReturn;
    }

    /// <summary>
    /// Clear grid spaces from certain item
    /// </summary>
    /// <param name="item"></param>
    private void CleanGridReference(ItemFromInventory item)
    {
        for (int i = 0; i < item.WIDTH; i++)
        {
            for (int j = 0; j < item.HEIGHT; j++)
            {
                if (item.spaceFill[i, j])
                {
                    inventoryItemsSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
                }

            }
        }
    }

    /// <summary>
    /// Checking all spaces to find available space for item 
    /// TO OPTIMILIZE
    /// </summary>
    /// <param name="itemToInsert"></param>
    /// <returns></returns>
    internal Vector2Int? FindSpaceForObject(ItemFromInventory itemToInsert)
    {
        for (int j = 0; j < gridSize.y; j++)
        {
            for(int i = 0; i < gridSize.x; i++)
            {
                if(CheckAvailableSpace(i, j, itemToInsert))
                {
                    return new Vector2Int(i,j);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Check if posX, posY is in the InventoryGrid
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
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
    /// <param name="posY"> range from 0 to gridSize.y and changed to negative value</param>
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
