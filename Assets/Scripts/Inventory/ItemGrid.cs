using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interaction with inventory Grid and creating it
/// </summary>
public class ItemGrid : MonoBehaviour
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

    private void Init()
    {
        inventoryItemsSlot = new ItemFromInventory[gridSize.x, gridSize.y];
        Vector2 size = new Vector2(gridSize.x * tileSizeWidth, gridSize.y * tileSizeHeight);
        rectTransform.sizeDelta = size;

        //Testing
        /*InventoryItem inventoryItem = Instantiate(inventoryItemPrefab.GetComponent<InventoryItem>());
        PlaceItem(inventoryItem, 3, 2);*/
    }

    public Vector2Int GetInvGridPositon(Vector2 mousePosition)
    {
        //Debug.Log($"mous {mousePosition.x} {mousePosition.y}");
        //Debug.Log($"rect {rectTransform.position.x} {rectTransform.position.y}");
        //Debug.Log($"local rect {rectTransform.localPosition.x} {rectTransform.localPosition.y}");

        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = mousePosition.y - rectTransform.position.y;

        tileGridPosition.x = (int) (positionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int) (positionOnTheGrid.y / tileSizeHeight);

        return tileGridPosition;
    }

    public bool PlaceItem(ItemFromInventory inventoryItem, int posX, int posY, ref ItemFromInventory overlapItem)
    {
        if(!BoundryCheck(posX,posY, inventoryItem.itemData.width, inventoryItem.itemData.height))
        {
            return false;
        }

        if(!OverlapCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        //Looks bad but constant Matrix4x4 in ItemData makes it proper(I think)
        for(int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (inventoryItem.itemData.fill[i, j] > 0)
                {
                    inventoryItemsSlot[posX + i, posY + j] = inventoryItem;
                    Debug.Log($"Place item in x{posX + i} y{posY + j}");
                }
                
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        rectTransform.parent = rectTransform;
        inventoryItemsSlot[posX, posY] = inventoryItem;

        Vector2 position = new Vector2();
        //position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.itemData.width / 2;
        //position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.itemData.height / 2);

        position.x = posX * tileSizeWidth;
        position.y = -(posY * tileSizeHeight);

        rectTransform.localPosition = position;

        return true;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref ItemFromInventory overlapItem)
    {
        for(int x = 0; x < width; x++)
        {
            for(var y = 0; y < height; y++)
            {
                if(inventoryItemsSlot[posX+x, posY+y] != null)
                {
                    if( overlapItem == null)
                    {
                        overlapItem = inventoryItemsSlot[posX + x, posY + y];
                    }
                    else
                    {
                        if(overlapItem != inventoryItemsSlot[posX+x, posY + y])
                        {
                            return false;
                        }
                        
                    }
                    
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
                if (item.itemData.fill[i, j] > 0)
                {
                    inventoryItemsSlot[item.onGridPositionX + i, item.onGridPositionY + j] = null;
                }

            }
        }
    }

    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= gridSize.x || posY > gridSize.y)
        {
            return false;
        }

        return true;
    }

    bool BoundryCheck(int posX, int posY, int width, int height)
    {
        if(!PositionCheck(posX, posY)) return false;

        posX += width-1;
        posY += height-1;

        if (!PositionCheck(posX, posY)) return false;

        return true;
    }

    
}
