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

    public void HidePlayerStash()
    {
        _playerBasics.SwitchInventory();
        _playerStash.SetActive(false);
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
