using Assets.Scripts.Enums;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    PlayerControl _playerControl;

    [SerializeField]
    InventoryManager _inventoryManager;

    [SerializeField] RoomManager _roomManager;

    [SerializeField]
    GameObject _playerStash;

    [SerializeField]
    InventoryGrid _itemsGrid;

    [SerializeField]
    EnvObject currentItemStash_Env;

    [SerializeField]
    Door[] _doors;

    [SerializeField] GameObject _inventoryWindow;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerControl == null)
        {
            _playerControl = FindObjectOfType<PlayerControl>();
        }
        if (_inventoryManager == null)
        {
            _inventoryManager = FindObjectOfType<InventoryManager>();
        }
        GetAllDoors();
    }

    public InteractionState GetPlayerSTATE()
    {
        return _playerControl.STATE;
    }

    //Player's Stash with items moving along all game
    public void ShowPlayerStash(EnvObject currentItemStash)
    {
        _playerControl.SwitchInventory(true);
        _playerStash.SetActive(true);
        currentItemStash_Env = currentItemStash;
    }

    public void ShowNormalStash(EnvObject currentItemStash)
    {
        if (_playerControl.STATE != InteractionState.INVENTORY)
            _playerControl.SwitchInventory(true);

        _inventoryManager.ShowItemStash();
        currentItemStash_Env = currentItemStash;
        _inventoryManager.FillItemsStash(currentItemStash_Env.Items);
    }

    internal void CheckForKey(Door door)
    {
        Debug.Log("Items in EQ " + _inventoryManager.PlayerInventory.ItemsOnGrid.Count);
        foreach(ItemFromInventory item in _inventoryManager.PlayerInventory.ItemsOnGrid)
        {
            if (item.itemData.id == door.KeyID)
            {
                Debug.Log("Using key " + item.ItemName);
                door.Closed = false;
            }
        }
    }

    public void HideInventory()
    {
        if (_inventoryManager.HoldedItem == null)
        {
            _playerControl.SwitchInventory(false);

            if (currentItemStash_Env.IsStash) _inventoryManager.HideItemStash(currentItemStash_Env);
            if (currentItemStash_Env.IsPlayerStash) _inventoryManager.HidePlayerStash();

            currentItemStash_Env.CloseSprite();
            currentItemStash_Env = null;

            HideInventoryWindow();
        }
        else Debug.Log("Can't close Inventory, item is holded");
    }

    public void ShowInventoryWindow()
    {
        _inventoryWindow.SetActive(true);
    }

    public void HideInventoryWindow()
    {
        _inventoryWindow.SetActive(false);
    }


    //Transport Player between doors
    public bool TransferPlayer(Door door)
    {
        if (_playerControl.STATE == InteractionState.DEFAULT)
        {
            if (_playerControl.PlayerMove.Agent.Warp(door.Destination.SpawnPoint.position))
            {
                ChangeCurrentRoom(door.Destination.ThisRoom);
                Debug.Log("Player moved to another room");
                return true;
            }
        }
        return false;
        //ANIMATION ACTIONS
    }

    public void ChangeCurrentRoom(Room room)
    {
        RoomManager.ChangeRoom(room);
    }

    private Door[] GetAllDoors()
    {
        if (_doors == null) _doors = FindObjectsOfType<Door>();
        return _doors;
    }



}
