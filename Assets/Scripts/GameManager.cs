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
        if (_playerBasics.STATE != Assets.Scripts.Enums.InteractionState.INVENTORY)
            _playerBasics.SwitchInventory(true);

        _inventoryManager.ShowItemStash();
        currentItemStash_Item = currentItemStash;
        //_inventoryManager.ClearItemsFromStash();
        _inventoryManager.FillItemsStash(currentItemStash_Item.Items);
    }

    public void HideInventory()
    {
        if(_inventoryManager.SelectedItem == null)
        {
            _playerBasics.SwitchInventory(false);

            if (currentItemStash_Item != null) _inventoryManager.HideItemStash(currentItemStash_Item);
            currentItemStash_Item = null;
            
            _inventoryManager.HidePlayerStash();
        }
        else Debug.Log("Can't close Inventory, item is holded");
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
