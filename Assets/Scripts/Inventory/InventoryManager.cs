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
    InventoryGrid itemGrid;

    [SerializeField] public List<ItemData> itemsList;
    [SerializeField] public GameObject itemPrefab;
    [SerializeField] public Transform canvasTransform;

    [SerializeField]
    public ItemFromInventory selectedItem;
    [SerializeField]
    ItemFromInventory overlapItem;
    [SerializeField]
    ItemFromInventory itemToHighlight;

    public InventoryGrid itemGRID {
        get
        {
            return itemGrid;
        }
        set
        {
            itemGrid = value;
        }
    }

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
    }

    // Update is called once per frame
    void Update()
    {
        //HandleHighlight();
    }

    public void HandleHighlight(Vector2 mousePos)
    {
        Vector2Int positionOnGrid = GetInvGridPositon(mousePos);
        Debug.Log("Handle " + positionOnGrid);
        if (selectedItem == null)
        {
            itemToHighlight = itemGRID.GetItem(positionOnGrid);
            if(itemToHighlight != null)
            {
                Debug.Log("Item to highlight " + itemToHighlight + itemToHighlight.itemDescription);

                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(itemGRID, itemToHighlight);
            }

        }
    }

    public Vector2Int GetInvGridPositon(Vector2 mousePosition)
    {
        itemGRID.PositionOnTheGrid = new Vector2(mousePosition.x - itemGRID.GridRectTransform.position.x, mousePosition.y - itemGRID.GridRectTransform.position.y);
        itemGRID.TileGridPosition = new Vector2Int ((int) (itemGRID.PositionOnTheGrid.x / InventoryGrid.tileSizeWidth), (int)(itemGRID.PositionOnTheGrid.y / InventoryGrid.tileSizeHeight));
        Debug.Log("Grid pos " + itemGRID.TileGridPosition);
        return itemGRID.TileGridPosition;
    }

    /// <summary>
    /// Method executed by PlayerInput MouseLClick (in reality not)
    /// </summary>
    /// <param name="mousePos"></param>
    public void GrabAndDropItemIcon(Vector2 mousePos)
    {
        if (itemGrid != null)
        {
            Vector2Int tileGridPosition = itemGRID.GetInvGridPositon(mousePos);
            Debug.Log(tileGridPosition.ToString());

            if (selectedItem == null)
            {
                GrabItemIcon(tileGridPosition);
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
        selectedItem = itemGrid.PickUpItem(tileGridPosition.x, Mathf.Abs(tileGridPosition.y));
        if (selectedItem != null)
        {
            currentItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void DropItemIcon(Vector2Int tileGridPosition)
    {
        bool dropComplete= itemGrid.PlaceItem(selectedItem, tileGridPosition.x, Mathf.Abs(tileGridPosition.y), ref overlapItem);
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

    public bool CheckMouseInInventory()
    {
        Vector2 mousePos = GetInvGridPositon(playerInput.UI.MousePosition.ReadValue<Vector2>());
        Vector2 gridsize = itemGrid.GridSize;
        Debug.Log($"CheckMouseInInventory mousePos {mousePos} vs gridsize {gridsize}");
        return mousePos.x >= 0 && mousePos.x < gridsize.x && mousePos.y <= 0 && mousePos.y > -gridsize.y ? true : false;
    }
}
