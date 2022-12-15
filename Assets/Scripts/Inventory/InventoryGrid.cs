using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interaction with inventory Grid and creating it
/// </summary>
public class InventoryGrid : MonoBehaviour
{
    public const float TileSizeWidth = 32;
    public const float TileSizeHeight = 32;

    RectTransform _rectTransform;
    Vector2 _positionOnTheGrid;
    Vector2Int _tileGridPosition;

    ItemFromInventory[,] _inventoryItemsSlot;

    [SerializeField]
    Vector2Int _gridSize;

    [SerializeField]
    GameObject _inventoryItemPrefab;

    public Vector2 PositionOnTheGrid { get => _positionOnTheGrid; set => _positionOnTheGrid = value; }
    public Vector2Int TileGridPosition { get => _tileGridPosition; set => _tileGridPosition = value; }
    public Vector2Int GridSize { get => _gridSize; set => _gridSize = value; }
    public ItemFromInventory[,] InventoryItemsSlot { get => _inventoryItemsSlot; set => _inventoryItemsSlot = value; }
    public RectTransform GridRectTransform { get => _rectTransform; set => _rectTransform = value; }
    public List<ItemFromInventory> ItemsOnGrid;

    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        Init();
        _positionOnTheGrid = new Vector2();
        _tileGridPosition = new Vector2Int();
        //gameObject.SetActive(false);
    }

    internal ItemFromInventory GetItem(Vector2Int positionOnGrid)
    {
        Debug.Log("GetItem from " + positionOnGrid);
        return _inventoryItemsSlot[positionOnGrid.x, -(positionOnGrid.y)];
    }

    private void Init()
    {
        _inventoryItemsSlot = new ItemFromInventory[_gridSize.x, _gridSize.y];
        Vector2 size = new Vector2(_gridSize.x * TileSizeWidth, _gridSize.y * TileSizeHeight);
        _rectTransform.sizeDelta = size;
    }

    public Vector2Int GetInvGridPositon(Vector2 mousePosition)
    {

        _positionOnTheGrid.x = mousePosition.x - _rectTransform.position.x;
        _positionOnTheGrid.y = mousePosition.y - _rectTransform.position.y;

        _tileGridPosition.x = (int) (_positionOnTheGrid.x / TileSizeWidth);
        _tileGridPosition.y = (int) (_positionOnTheGrid.y / TileSizeHeight);

        return _tileGridPosition;
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
        rectTransform.SetParent(this._rectTransform);

        for (int i = 0; i < inventoryItem.WIDTH; i++)
        {
            for (int j = 0; j < inventoryItem.HEIGHT; j++)
            {

                if (inventoryItem.SpaceFill[i, j])
                {
                    Debug.Log($"Place item in x{posX + i} y{posY + j}");
                    _inventoryItemsSlot[posX + i, posY + j] = inventoryItem;
                    ItemsOnGrid.Add(inventoryItem);
                }

            }
        }

        inventoryItem.OnGridPositionX = posX;
        inventoryItem.OnGridPositionY = posY;

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

        position.x = posX * TileSizeWidth;
        position.y = -(posY * TileSizeHeight);
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
                if (holdedItem.SpaceFill[x, y])
                {
                    try
                    {   
                    
                        if (_inventoryItemsSlot[posX + x, posY + y] != null)
                        {
                            if (overlapItem == null)
                            {
                                overlapItem = _inventoryItemsSlot[posX + x, posY + y];
                            }
                            else
                            {
                                if (overlapItem != _inventoryItemsSlot[posX + x, posY + y])
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
                    if (holdedItem.SpaceFill[x, y])
                    {


                        if (_inventoryItemsSlot[posX + x, posY + y] != null)
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
        ItemFromInventory toReturn = _inventoryItemsSlot[x, y];

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
                if (item.SpaceFill[i, j])
                {
                    _inventoryItemsSlot[item.OnGridPositionX + i, item.OnGridPositionY + j] = null;
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
        for (int j = 0; j < _gridSize.y; j++)
        {
            for(int i = 0; i < _gridSize.x; i++)
            {
                Debug.Log("FindSpaceForObject j " + j);
                Debug.Log("FindSpaceForObject i " + i );
                Debug.Log("FindSpaceForObject x " + itemToInsert);
                if (CheckAvailableSpace(i, j, itemToInsert))
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

        if (posX >= _gridSize.x || posY <= -_gridSize.y)
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
