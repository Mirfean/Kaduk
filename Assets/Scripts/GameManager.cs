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

    //Player's Stash with items moving along all game
    public void ShowPlayerStash()
    {
        _playerBasics.SwitchInventory();
        _playerStash.SetActive(true);
    }

    public void ShowNormalStash(List<ItemData> items)
    {
        _playerBasics.SwitchInventory();
        _itemsGrid.gameObject.SetActive(true);
        foreach (ItemData item in items)
        {
            _inventoryManager.InsertCertainItem(item, _itemsGrid);
        }
        

    }

    public void HideInventory()
    {
        if(_inventoryManager.SelectedItem == null)
        {
            _playerBasics.SwitchInventory();
            _playerStash.SetActive(false);
            _itemsGrid.gameObject.SetActive(false);
        }
        Debug.Log("Can't close Inventory, item is holded");
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
