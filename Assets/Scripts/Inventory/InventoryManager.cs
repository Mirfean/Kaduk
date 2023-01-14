using Assets.Scripts.Enums;
using System;
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

    [SerializeField] public ItemFromInventory HoldedItem;

    [SerializeField] public ItemFromInventory ClickedItem;
    
    [SerializeField] public ItemFromInventory OnMouseItem;
    
    [SerializeField] ItemFromInventory _overlapItem;
    
    [SerializeField] ItemFromInventory _itemToHighlight;

    [SerializeField]
    Vector2 _oldPositon;
    
    [SerializeField]
    public RectTransform CurrentItemRectTransform;

    [SerializeField]
    InventoryHighlight _inventoryHighlight;

    public static Action<ItemFromInventory> OnMouseAboveItem;

    EquippedWeapon _equipedWeapon;

    private void OnEnable()
    {
        OnMouseAboveItem += SetOnMouseItem;
    }

    private void OnDisable()
    {
        OnMouseAboveItem -= SetOnMouseItem;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = FindObjectOfType<PlayerControl>().PlayerInput;
        _playerInput.UI.SpawnItem.performed += ctx => SpawnRandomItem(ctx);
        _playerInput.UI.InsertItem.performed += ctx => InsertRandomItem(ctx);
        _playerInput.UI.RotateItem.performed += ctx => RotateHoldedItem(ctx);

        _inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    public void SetOnMouseItem(ItemFromInventory item)
    {
         OnMouseItem = item;
    }

    public InventoryGrid GetSecondGrid(StashType stashType)
    {
        switch (stashType)
        {
            case StashType.INVENTORY:
                if (_playerStash.gameObject.activeSelf) return _playerStash;
                else if (_itemsStash.gameObject.activeSelf) return _itemsStash;
                break;
            case StashType.PLAYER_STASH:
                return _playerInventory;
            case StashType.ITEMSTASH:
                return _playerInventory;
        }

        if(SelectedItemGRID == _playerInventory)
        {
            
        }
        else if (SelectedItemGRID == _playerStash || SelectedItemGRID == _itemsStash)
        {
            return _playerInventory;
        }
        Debug.Log("There is only one grid");
        return SelectedItemGRID;
     
    }

    public void ChangeGridForHoldedItem(StashType currentStashType)
    {
        CreateAndInsertCertainItem(HoldedItem.itemData, GetSecondGrid(currentStashType));
        Destroy(HoldedItem.gameObject);
        HoldedItem = null;
    }

    public void ChangeGridForItem(StashType currentStashType)
    {
        CreateAndInsertCertainItem(ClickedItem.itemData, GetSecondGrid(currentStashType));
        Destroy(ClickedItem.gameObject);
        ClickedItem = null;
        OnMouseItem = null;
        HoldedItem = null;
    }

    public void HandleHighlight(Vector2 mousePos)
    {
        Vector2Int positionOnGrid = GetInvGridPositon(mousePos);
        if (_oldPositon == positionOnGrid || positionOnGrid == new Vector2Int(-1, -1)) return;
        
        _oldPositon = positionOnGrid;
        if (HoldedItem == null)
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
            _inventoryHighlight.Show(SelectedItemGRID.BoundryCheck(positionOnGrid.x, -positionOnGrid.y, HoldedItem.WIDTH, HoldedItem.HEIGHT));
            _inventoryHighlight.SetSize(HoldedItem);
            _inventoryHighlight.SetParent(SelectedItemGRID);
            _inventoryHighlight.SetPosition(SelectedItemGRID, HoldedItem, positionOnGrid.x, -positionOnGrid.y);
        }
    }

    public Vector2Int GetInvGridPositon(Vector2 mousePosition)
    {
        if (HoldedItem == null || SelectedItemGRID == null) {  return new Vector2Int(-1, -1); }
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
            HoldedItem = _selectedItemGrid.PickUpItem(tileGridPosition.x, Mathf.Abs(tileGridPosition.y));
            if (HoldedItem != null)
            {
                _selectedItemGrid.ItemsOnGrid.Remove(HoldedItem);
                CurrentItemRectTransform = HoldedItem.GetComponent<RectTransform>();
            }
        }

        void DropItemIcon(Vector2Int tileGridPosition)
        {
            bool dropComplete = _selectedItemGrid.IsThisPlaceForItem(HoldedItem, tileGridPosition.x, Mathf.Abs(tileGridPosition.y), ref _overlapItem);
            if (HoldedItem._inventorygrid != _selectedItemGrid)
            {
                HoldedItem.transform.SetParent(_selectedItemGrid.transform, false);
            }
            _selectedItemGrid.PlaceItemToGrid(HoldedItem, tileGridPosition.x, -tileGridPosition.y);
            HoldedItem = null;
            if (dropComplete)
            {
                //SelectedItem = null;
                if (_overlapItem != null)
                {
                    HoldedItem = _overlapItem;
                    _overlapItem = null;
                    CurrentItemRectTransform = HoldedItem.GetComponent<RectTransform>();
                }
                else
                {
                    CurrentItemRectTransform = null;
                }
            }

        }

        if (_selectedItemGrid != null)
        {
            Vector2Int tileGridPosition = SelectedItemGRID.GetInvGridPositon(mousePos);
            Debug.Log($"On grid {SelectedItemGRID} position {tileGridPosition.ToString()}");

            if (HoldedItem == null)
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

    bool CheckIfItemsFromSameGrid(ItemFromInventory holded, ItemFromInventory overlaped)
    {
        return holded._inventorygrid == overlaped._inventorygrid;
    }

    /// <summary>
    /// Move clicked item icon across inventory following cursor
    /// </summary>
    /// <param name="mousePos"></param>
    public void MoveItemIcon(Vector2 mousePos)
    {
        if (CurrentItemRectTransform != null) CurrentItemRectTransform.position = mousePos + new Vector2(1, -1);
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
        if (HoldedItem != null)
        {
            HoldedItem.rotate();
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
        Debug.Log($"Size {_itemsStash.InventoryItemsSlot.Length}");
    }

    public void ClearItemsFromStash()
    {
        ItemFromInventory[] ItemsInInventory = _itemsStash.GetComponentsInChildren<ItemFromInventory>();
        if (ItemsInInventory.Length == 0) return;
        for (int i = 0; i < ItemsInInventory.Length; i++)
        {
            Destroy(ItemsInInventory[i].gameObject);
        }
        _itemsStash.CleanAllGridReferences();
    }



    #region Random Item Spawner
    public void SpawnRandomItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(ItemPrefab).GetComponent<ItemFromInventory>();
        HoldedItem = item;
        
        CurrentItemRectTransform = item.GetComponent<RectTransform>();
        CurrentItemRectTransform.SetParent(InventoryCanvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, ItemsList.Count);
        //item.itemData = 
        HoldedItem.itemData = ItemsList[selectedItemID];

        Debug.Log($"Spawn {HoldedItem}");
    }

    public void SpawnRandomItem()
    {
        ItemFromInventory item = Instantiate(ItemPrefab).GetComponent<ItemFromInventory>();
        HoldedItem = item;

        CurrentItemRectTransform = item.GetComponent<RectTransform>();
        CurrentItemRectTransform.SetParent(InventoryCanvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, ItemsList.Count);
        //item.itemData = 
        HoldedItem.itemData = ItemsList[selectedItemID];

        Debug.Log($"Spawn {HoldedItem}");
    }
    
    public void InsertRandomItem(InputAction.CallbackContext context)
    {
        if (_selectedItemGrid == null) return;

        Debug.Log("Insert random");
        SpawnRandomItem();
        ItemFromInventory itemToInsert = HoldedItem;
        HoldedItem = null;
        InsertCreatedItemOnSelectedGrid(itemToInsert);
    }
    void InsertCreatedItemOnSelectedGrid(ItemFromInventory itemToInsert)
    {
        Vector2Int? posOnGrid = SelectedItemGRID.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null) { return; }

        _selectedItemGrid.PlaceItemToGrid(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }
    #endregion

/*  ??????
    ??????
    ??????
    ?????????
    ????????*/


    #region Item Spawner
    public void CreateAndInsertCertainItem(ItemData itemData, InventoryGrid grid)
    {
        SelectedItemGRID = grid;
        ItemFromInventory itemFrom = SpawnItem(itemData);
        InsertCreatedItem(itemFrom, grid);

        //SelectedItem = null;
        //CurrentItemRectTransform = null;

        ItemFromInventory SpawnItem(ItemData itemData)
        {
            GameObject newItem = Instantiate(ItemPrefab);
            ItemFromInventory item = newItem.GetComponent<ItemFromInventory>();
            HoldedItem = item;
            CurrentItemRectTransform = HoldedItem.GetComponent<RectTransform>();
            CurrentItemRectTransform.SetParent(grid.GetComponent<RectTransform>());

            Debug.Log("Created item " + HoldedItem.gameObject.transform.position);

            HoldedItem.itemData = itemData;
            Debug.Log($"Spawn {item.name}");
            return HoldedItem;
        }

        void InsertCreatedItem(ItemFromInventory itemToInsert, InventoryGrid grid)
        {
            Vector2Int? posOnGrid = grid.FindSpaceForObject(itemToInsert);
            if (posOnGrid == null) { return; }
            grid.PlaceItemToGrid(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
            HoldedItem = null;
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
        ItemFromInventory[] ItemsInInventory = _itemsStash.GetComponentsInChildren<ItemFromInventory>();
        Debug.Log("ItemsInInventory " + ItemsInInventory.Length);
        List<ItemData> newItems = new List<ItemData>();
        
        if (ItemsInInventory.Length == 0) return;
        
        for (int i = 0; i < ItemsInInventory.Length; i++)
        {
            newItems.Add(ItemsInInventory[i].itemData);
        }

        currentItemStash.Items = newItems;
    }

    public void ChangeCurrentWeapon(GameObject newWeapon)
    {
        FindObjectOfType<PlayerWeapon>().ChangeWeapon(newWeapon.GetComponent<ItemFromInventory>().itemData.Weapon.WeaponPrefab);
        _equipedWeapon.ChangeWeapon(newWeapon.GetComponent<_Weapon>());
    }
}
