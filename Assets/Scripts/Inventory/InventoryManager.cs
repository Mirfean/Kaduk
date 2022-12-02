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
    Player _playerInput;

    [SerializeField]
    InventoryGrid _selectedItemGrid;

    public InventoryGrid SelectedItemGRID
    {
        get
        {
            return _selectedItemGrid;
        }
        set
        {
            _selectedItemGrid = value;
            _inventoryHighlight.SetParent(value);
        }
    }

    [SerializeField] public List<ItemData> ItemsList;
    [SerializeField] public GameObject ItemPrefab;
    [SerializeField] public Transform CanvasTransform;

    [SerializeField]
    public ItemFromInventory SelectedItem;
    [SerializeField]
    ItemFromInventory _overlapItem;
    [SerializeField]
    ItemFromInventory _itemToHighlight;

    [SerializeField]
    Vector2 _oldPositon;
    

    [SerializeField]
    public RectTransform CurrentItemRectTransform;

    [SerializeField]
    InventoryHighlight _inventoryHighlight;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = new Player();
        _playerInput.Enable();
        _playerInput.UI.SpawnItem.performed += ctx => SpawnRandomItem(ctx);
        _playerInput.UI.InsertItem.performed += ctx => InsertRandomItem(ctx);
        _playerInput.UI.RotateItem.performed += ctx => RotateHoldedItem(ctx);

        _inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    public void HandleHighlight(Vector2 mousePos)
    {
        Vector2Int positionOnGrid = GetInvGridPositon(mousePos);
        if (_oldPositon == positionOnGrid || positionOnGrid == new Vector2Int(-1, -1)) return;
        
        _oldPositon = positionOnGrid;
        if (SelectedItem == null)
        {
            _itemToHighlight = SelectedItemGRID.GetItem(positionOnGrid);
            if(_itemToHighlight != null)
            {
                Debug.Log("Item to highlight " + _itemToHighlight + _itemToHighlight.ItemDescription);

                _inventoryHighlight.Show(true);
                _inventoryHighlight.SetSize(_itemToHighlight);
                _inventoryHighlight.SetParent(SelectedItemGRID);
                _inventoryHighlight.SetPosition(SelectedItemGRID, _itemToHighlight);
            }
            else
            {
                if (_inventoryHighlight.HighlighterRectTrans.gameObject.activeSelf)
                {
                    _inventoryHighlight.Show(false);
                }
                
            }

        }
        else
        {
            _inventoryHighlight.Show(SelectedItemGRID.BoundryCheck(positionOnGrid.x, -positionOnGrid.y, SelectedItem.WIDTH, SelectedItem.HEIGHT));
            _inventoryHighlight.SetSize(SelectedItem);
            _inventoryHighlight.SetParent(SelectedItemGRID);
            _inventoryHighlight.SetPosition(SelectedItemGRID, SelectedItem, positionOnGrid.x, -positionOnGrid.y);
        }
    }

    public Vector2Int GetInvGridPositon(Vector2 mousePosition)
    {
        if (SelectedItem == null) {  return new Vector2Int(-1, -1); }
        SelectedItemGRID.PositionOnTheGrid = new Vector2(mousePosition.x - SelectedItemGRID.GridRectTransform.position.x, mousePosition.y - SelectedItemGRID.GridRectTransform.position.y);
        SelectedItemGRID.TileGridPosition = new Vector2Int ((int) (SelectedItemGRID.PositionOnTheGrid.x / InventoryGrid.TileSizeWidth), (int)(SelectedItemGRID.PositionOnTheGrid.y / InventoryGrid.TileSizeHeight));
        return SelectedItemGRID.TileGridPosition;
    }

    /// <summary>
    /// Method executed by PlayerInput MouseLClick (in reality not)
    /// </summary>
    /// <param name="mousePos"></param>
    public void GrabAndDropItemIcon(Vector2 mousePos)
    {
        if (_selectedItemGrid != null)
        {
            Vector2Int tileGridPosition = SelectedItemGRID.GetInvGridPositon(mousePos);
            Debug.Log(tileGridPosition.ToString());

            if (SelectedItem == null)
            {
                if (_selectedItemGrid.PositionCheck(tileGridPosition.x, tileGridPosition.y))
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
        SelectedItem = _selectedItemGrid.PickUpItem(tileGridPosition.x, Mathf.Abs(tileGridPosition.y));
        if (SelectedItem != null)
        {
            CurrentItemRectTransform = SelectedItem.GetComponent<RectTransform>();
        }
    }

    private void DropItemIcon(Vector2Int tileGridPosition)
    {
        bool dropComplete= _selectedItemGrid.PlaceItem(SelectedItem, tileGridPosition.x, Mathf.Abs(tileGridPosition.y), ref _overlapItem);
        if (dropComplete)
        {
            SelectedItem = null;
            if (_overlapItem != null)
            {
                SelectedItem = _overlapItem;
                _overlapItem = null;
                CurrentItemRectTransform = SelectedItem.GetComponent<RectTransform>();
            }
        }
        
    }

    /// <summary>
    /// Move clicked item icon across inventory following cursor
    /// </summary>
    /// <param name="mousePos"></param>
    public void MoveItemIcon(Vector2 mousePos)
    {
        if (SelectedItem != null)
        {
            CurrentItemRectTransform.position = mousePos;
        }
    }

    public void SpawnRandomItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(ItemPrefab).GetComponent<ItemFromInventory>();
        SelectedItem = item;
        
        CurrentItemRectTransform = item.GetComponent<RectTransform>();
        CurrentItemRectTransform.SetParent(CanvasTransform);

        int selectedItemID = Random.Range(0, ItemsList.Count);
        //item.itemData = 
        SelectedItem.itemData = ItemsList[selectedItemID];

        Debug.Log($"Spawn {SelectedItem}");
    }

    public void SpawnRandomItem()
    {
        ItemFromInventory item = Instantiate(ItemPrefab).GetComponent<ItemFromInventory>();
        SelectedItem = item;

        CurrentItemRectTransform = item.GetComponent<RectTransform>();
        CurrentItemRectTransform.SetParent(CanvasTransform);

        int selectedItemID = Random.Range(0, ItemsList.Count);
        //item.itemData = 
        SelectedItem.itemData = ItemsList[selectedItemID];

        Debug.Log($"Spawn {SelectedItem}");
    }

    public void InsertRandomItem(InputAction.CallbackContext context)
    {
        if (_selectedItemGrid == null) return;

        Debug.Log("Insert random");
        SpawnRandomItem();
        ItemFromInventory itemToInsert = SelectedItem;
        SelectedItem = null;
        InsertItem(itemToInsert);
    }

    void InsertItem(ItemFromInventory itemToInsert)
    {
        Vector2Int? posOnGrid = SelectedItemGRID.FindSpaceForObject(itemToInsert);

        if(posOnGrid == null) { return; }

        _selectedItemGrid.PlaceItemToGrid(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    public bool CheckMouseInInventory()
    {
        Vector2 mousePos = GetInvGridPositon(_playerInput.UI.MousePosition.ReadValue<Vector2>());
        if (mousePos == new Vector2(-1, -1)) return false;
        Vector2 gridsize = _selectedItemGrid.GridSize;
        //Debug.Log($"CheckMouseInInventory mousePos {mousePos} vs gridsize {gridsize}");
        return mousePos.x >= 0 && mousePos.x < gridsize.x && mousePos.y <= 0 && mousePos.y > -gridsize.y ? true : false;
    }

    public void RotateHoldedItem(InputAction.CallbackContext context)
    {
        Debug.Log("rotating");
        if(SelectedItem != null)
        {
            SelectedItem.rotate();
        }
        
    }
}
