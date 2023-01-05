using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    PlayerBasics _playerBasics;

    [SerializeField]
    InventoryManager _inventoryManager;

    [SerializeField]
    GameObject _playerStash;

    [SerializeField]
    InventoryGrid _itemsGrid;

    [SerializeField]
    _Item currentItemStash_Item;

    [SerializeField]
    Door[] _doors;

    [SerializeField]
    public static Vector2 MousePosition;

    [SerializeField] GameObject _inventoryWindow;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerBasics == null)
        {
            _playerBasics = FindObjectOfType<PlayerBasics>();
        }
        if (_inventoryManager == null)
        {
            _inventoryManager = FindObjectOfType<InventoryManager>();
        }
        GetAllDoors();
    }

    // Update is called once per frame
    void Update()
    {
        MousePosition = _playerBasics.GetMousePos();
    }

    public InteractionState GetPlayerSTATE()
    {
        return _playerBasics.STATE;
    }

    //Player's Stash with items moving along all game
    public void ShowPlayerStash()
    {
        _playerBasics.SwitchInventory(true);
        _playerStash.SetActive(true);
    }

    public void ShowNormalStash(_Item currentItemStash)
    {
        if (_playerBasics.STATE != InteractionState.INVENTORY)
            _playerBasics.SwitchInventory(true);

        _inventoryManager.ShowItemStash();
        currentItemStash_Item = currentItemStash;
        //_inventoryManager.ClearItemsFromStash();
        _inventoryManager.FillItemsStash(currentItemStash_Item.Items);
    }

    public void HideInventory()
    {
        if(_inventoryManager.HoldedItem == null)
        {
            _playerBasics.SwitchInventory(false);

            if (currentItemStash_Item != null) _inventoryManager.HideItemStash(currentItemStash_Item);
            currentItemStash_Item = null;
            
            _inventoryManager.HidePlayerStash();

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
    internal void TransferPlayer(Door door)
    {
        //ANIMATION ACTIONS
        if (_playerBasics.PlayerMove.Agent.Warp(door.Destination.SpawnPoint.position))
        {
            Debug.Log("Player moved to another room");
        }
    }

    private Door[] GetAllDoors()
    {
        if (_doors == null) _doors = FindObjectsOfType<Door>();
        return _doors;
    }



}
