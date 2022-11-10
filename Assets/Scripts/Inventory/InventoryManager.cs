using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Core of Inventory
/// Handle input
/// </summary>
public class InventoryManager : MonoBehaviour
{
    Player playerInput;

    [SerializeField]
    InventoryGrid selectedItemGrid;

    public InventoryGrid SelectedItemGRID
    {
        get
        {
            return selectedItemGrid;
        }
        set
        {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    [SerializeField] public List<ItemData> itemsList;
    [SerializeField] public GameObject itemPrefab;
    [SerializeField] public Transform canvasTransform;

    [SerializeField]
    public ItemFromInventory selectedItem;
    [SerializeField]
    ItemFromInventory overlapItem;
    [SerializeField]
    ItemFromInventory itemToHighlight;

    [SerializeField]
    Vector2 oldPositon;
    

    [SerializeField]
    public RectTransform currentItemRectTransform;

    [SerializeField]
    InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        //inventoryHighlight = GetComponent<InventoryHighlight>();
    }


    // Start is called before the first frame update
    void Start()
    {
        playerInput = new Player();
        playerInput.Enable();
        playerInput.UI.SpawnItem.performed += ctx => SpawnRandomItem(ctx);
        playerInput.UI.InsertItem.performed += ctx => InsertRandomItem(ctx);

        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    // Update is called once per frame
    void Update()
    {
        //HandleHighlight();
    }

    public void HandleHighlight(Vector2 mousePos)
    {
        Vector2Int positionOnGrid = GetInvGridPositon(mousePos);
        if (oldPositon == positionOnGrid) return;
        
        oldPositon = positionOnGrid;
        if (selectedItem == null)
        {
            itemToHighlight = SelectedItemGRID.GetItem(positionOnGrid);
            if(itemToHighlight != null)
            {
                Debug.Log("Item to highlight " + itemToHighlight + itemToHighlight.itemDescription);

                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetParent(SelectedItemGRID);
                inventoryHighlight.SetPosition(SelectedItemGRID, itemToHighlight);
            }
            else
            {
                if (inventoryHighlight.highlighter.gameObject.activeSelf)
                {
                    inventoryHighlight.Show(false);
                }
                
            }

        }
        else
        {
            inventoryHighlight.Show(SelectedItemGRID.BoundryCheck(positionOnGrid.x, -positionOnGrid.y, selectedItem.itemData.width, selectedItem.itemData.height));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetParent(SelectedItemGRID);
            inventoryHighlight.SetPosition(SelectedItemGRID, selectedItem, positionOnGrid.x, -positionOnGrid.y);
        }
    }

    public Vector2Int GetInvGridPositon(Vector2 mousePosition)
    {
        SelectedItemGRID.PositionOnTheGrid = new Vector2(mousePosition.x - SelectedItemGRID.GridRectTransform.position.x, mousePosition.y - SelectedItemGRID.GridRectTransform.position.y);
        SelectedItemGRID.TileGridPosition = new Vector2Int ((int) (SelectedItemGRID.PositionOnTheGrid.x / InventoryGrid.tileSizeWidth), (int)(SelectedItemGRID.PositionOnTheGrid.y / InventoryGrid.tileSizeHeight));
        //Debug.Log("Grid pos " + itemGRID.TileGridPosition);
        return SelectedItemGRID.TileGridPosition;
    }

    /// <summary>
    /// Method executed by PlayerInput MouseLClick (in reality not)
    /// </summary>
    /// <param name="mousePos"></param>
    public void GrabAndDropItemIcon(Vector2 mousePos)
    {
        if (selectedItemGrid != null)
        {
            Vector2Int tileGridPosition = SelectedItemGRID.GetInvGridPositon(mousePos);
            Debug.Log(tileGridPosition.ToString());

            if (selectedItem == null)
            {
                if (selectedItemGrid.PositionCheck(tileGridPosition.x, tileGridPosition.y))
                {
                    GrabItemIcon(tileGridPosition);
                }
                
                //Show Highlighter
            }
            else
            {
                DropItemIcon(tileGridPosition);
            }
        }
    }

    private void GrabItemIcon(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, Mathf.Abs(tileGridPosition.y));
        if (selectedItem != null)
        {
            currentItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void DropItemIcon(Vector2Int tileGridPosition)
    {
        bool dropComplete= selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, Mathf.Abs(tileGridPosition.y), ref overlapItem);
        if (dropComplete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                currentItemRectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
        
    }

    /// <summary>
    /// Move clicked item icon across inventory following cursor
    /// </summary>
    /// <param name="mousePos"></param>
    public void MoveItemIcon(Vector2 mousePos)
    {
        if (selectedItem != null)
        {
            currentItemRectTransform.position = mousePos;
        }
    }

    public void SpawnRandomItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(itemPrefab).GetComponent<ItemFromInventory>();
        selectedItem = item;
        
        currentItemRectTransform = item.GetComponent<RectTransform>();
        currentItemRectTransform.SetParent(canvasTransform);

        int selectedItemID = Random.Range(0, itemsList.Count);
        //item.itemData = 
        selectedItem.itemData = itemsList[selectedItemID];

        Debug.Log($"Spawn {selectedItem}");
    }

    public void SpawnRandomItem()
    {
        ItemFromInventory item = Instantiate(itemPrefab).GetComponent<ItemFromInventory>();
        selectedItem = item;

        currentItemRectTransform = item.GetComponent<RectTransform>();
        currentItemRectTransform.SetParent(canvasTransform);

        int selectedItemID = Random.Range(0, itemsList.Count);
        //item.itemData = 
        selectedItem.itemData = itemsList[selectedItemID];

        Debug.Log($"Spawn {selectedItem}");
    }

    public void InsertRandomItem(InputAction.CallbackContext context)
    {
        if (selectedItemGrid == null) return;

        Debug.Log("Insert random");
        SpawnRandomItem();
        ItemFromInventory itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    void InsertItem(ItemFromInventory itemToInsert)
    {
        Vector2Int? posOnGrid = SelectedItemGRID.FindSpaceForObject(itemToInsert);

        if(posOnGrid == null) { return; }

        selectedItemGrid.PlaceItemToGrid(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    public bool CheckMouseInInventory()
    {
        Vector2 mousePos = GetInvGridPositon(playerInput.UI.MousePosition.ReadValue<Vector2>());
        Vector2 gridsize = selectedItemGrid.GridSize;
        //Debug.Log($"CheckMouseInInventory mousePos {mousePos} vs gridsize {gridsize}");
        return mousePos.x >= 0 && mousePos.x < gridsize.x && mousePos.y <= 0 && mousePos.y > -gridsize.y ? true : false;
    }
}
