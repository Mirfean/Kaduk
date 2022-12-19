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

    [SerializeField]
    InventoryGrid _playerInventory;

    [SerializeField]
    InventoryGrid _playerStash;

    [SerializeField]
    InventoryGrid _itemsStash;

    [SerializeField] public Transform InventoryCanvasTransform;

    [SerializeField] public List<ItemData> ItemsList;
    [SerializeField] public GameObject ItemPrefab;

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
    /// Method executed by PlayerInput MouseLClick
    /// </summary>
    /// <param name="mousePos"></param>
    public void GrabAndDropItemIcon(Vector2 mousePos)
    {
        void GrabItemIcon(Vector2Int tileGridPosition)
        {
            SelectedItem = _selectedItemGrid.PickUpItem(tileGridPosition.x, Mathf.Abs(tileGridPosition.y));
            if (SelectedItem != null)
            {
                CurrentItemRectTransform = SelectedItem.GetComponent<RectTransform>();
            }
        }

        void DropItemIcon(Vector2Int tileGridPosition)
        {
            bool dropComplete = _selectedItemGrid.PlaceItem(SelectedItem, tileGridPosition.x, Mathf.Abs(tileGridPosition.y), ref _overlapItem);
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

    /// <summary>
    /// Move clicked item icon across inventory following cursor
    /// </summary>
    /// <param name="mousePos"></param>
    public void MoveItemIcon(Vector2 mousePos)
    {
        /*if (SelectedItem != null)
          {
              CurrentItemRectTransform.position = mousePos;
          }*/
        CurrentItemRectTransform.position = mousePos;
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
        if (SelectedItem != null)
        {
            SelectedItem.rotate();
        }

    }

    public void ReturnItemToLastPosition()
    {
        Debug.Log("Return item to last position - TODO");
    }

    public void FillItemsStash(List<ItemData> items)
    {
        Debug.Log("Items in list " + items.Count);
        foreach (ItemData item in items)
        {
            CreateAndInsertCertainItem(item, _itemsStash);
        }
    }

    public void ClearItemsFromStash()
    {
        if (_itemsStash.ItemsOnGrid.Count == 0) return;
        for (int i = 0; i < _itemsStash.ItemsOnGrid.Count; i++)
        {
            Destroy(_itemsStash.ItemsOnGrid[i].gameObject);
        }
        _itemsStash.ItemsOnGrid.Clear();
        _itemsStash.CleanAllGridReferences();
    }

    #region Random Item Spawner
    public void SpawnRandomItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(ItemPrefab).GetComponent<ItemFromInventory>();
        SelectedItem = item;
        
        CurrentItemRectTransform = item.GetComponent<RectTransform>();
        CurrentItemRectTransform.SetParent(InventoryCanvasTransform);

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
        CurrentItemRectTransform.SetParent(InventoryCanvasTransform);

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
        InsertCreatedItemOnSelectedGrid(itemToInsert);
    }
    void InsertCreatedItemOnSelectedGrid(ItemFromInventory itemToInsert)
    {
        Vector2Int? posOnGrid = SelectedItemGRID.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null) { return; }

        _selectedItemGrid.PlaceItemToGrid(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }
    #endregion

    #region Item Spawner
    public void CreateAndInsertCertainItem(ItemData itemData, InventoryGrid grid)
    {
        ItemFromInventory itemFrom = SpawnItem(itemData);

        InsertCreatedItem(itemFrom, grid);

        ItemFromInventory SpawnItem(ItemData itemData)
        {
            GameObject newItem = Instantiate(ItemPrefab);
            ItemFromInventory item = newItem.GetComponent<ItemFromInventory>();
            SelectedItem = item;
            CurrentItemRectTransform = item.GetComponent<RectTransform>();
            CurrentItemRectTransform.SetParent(grid.GetComponent<RectTransform>());

            Debug.Log("Created item " + item.gameObject.transform.position);

            item.itemData = itemData;
            Debug.Log($"Spawn {item.name}");
            return item;
        }

        void InsertCreatedItem(ItemFromInventory itemToInsert, InventoryGrid grid)
        {
            Vector2Int? posOnGrid = grid.FindSpaceForObject(itemToInsert);

            grid.PlaceItemToGrid(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        }
    }

    
    #endregion

    #region Grids SHOW/HIDE
    public void ShowInventory()
    {
        _playerInventory.gameObject.SetActive(true);
    }

    public void HideInventory()
    {
        _playerInventory.gameObject.SetActive(false);
    }

    public void ShowPlayerStash()
    {
        _playerStash.gameObject.SetActive(true);
    }

    public void HidePlayerStash()
    {
        _playerStash.gameObject.SetActive(false);
        //Save that?
    }

    public void ShowItemStash()
    {
        _itemsStash.gameObject.SetActive(true);
        _itemsStash.InventoryItemsSlot = new ItemFromInventory[_itemsStash.GridSize.x, _itemsStash.GridSize.y];
    }

    public void HideItemStash(_Item currentItemStash)
    {
        Debug.Log("Hide Item Stash");
        SaveChangesInItemStash(currentItemStash);
        ClearItemsFromStash();
        _itemsStash.gameObject.SetActive(false);
    }
    #endregion

    private void SaveChangesInItemStash(_Item currentItemStash)
    {
        List<ItemData> newItems = new List<ItemData>();
        for (int i = 0; i < _itemsStash.ItemsOnGrid.Count; i++)
        {
            newItems.Add(_itemsStash.ItemsOnGrid[i].itemData);
        }
        Debug.Log("ItemsOnGrid " + _itemsStash.ItemsOnGrid.Count);
        Debug.Log("newItems " + newItems.Count);
        currentItemStash.Items = newItems;
    }

    
}
