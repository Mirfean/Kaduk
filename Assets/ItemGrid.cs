using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    const float tileSizeWidth = 32;
    const float tileSizeHeight = 32;

    RectTransform rectTransform;
    Vector2 positionOnTheGrid;
    Vector2Int tileGridPosition;

    InventoryItem[,] inventoryItemsSlot;

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
        inventoryItemsSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;

        //Testing
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab.GetComponent<InventoryItem>());
        PlaceItem(inventoryItem, 3, 2);
    }

    private void Init()
    {
        inventoryItemsSlot = new InventoryItem[gridSize.x, gridSize.y];
        Vector2 size = new Vector2(gridSize.x * tileSizeWidth, gridSize.y * tileSizeHeight);
        rectTransform.sizeDelta = size;

        //Testing
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab.GetComponent<InventoryItem>());
        PlaceItem(inventoryItem, 3, 2);
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

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        rectTransform.parent = rectTransform;
        inventoryItemsSlot[posX, posY] = inventoryItem;

        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight / 2);

        rectTransform.localPosition = position;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemsSlot[x, y];
        inventoryItemsSlot[x, y] = null;
        return toReturn;
    }
}
