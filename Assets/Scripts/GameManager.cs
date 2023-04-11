using Assets.Scripts.Enums;
using System;
using System.Collections;
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

    public static Action<EnvObject> HandleEnvAction;

    public static Action<Door> HandleDoorAction;


    private void OnEnable()
    {
        HandleEnvAction += HandleCloseToEnvObject;
        HandleDoorAction += HandleCloseToDoor;
    }

    private void OnDisable()
    {
        HandleEnvAction -= HandleCloseToEnvObject;
        HandleDoorAction -= HandleCloseToDoor;
    }

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
        _inventoryManager.FillStackItemsStash(currentItemStash_Env.StackItems, currentItemStash_Env.StackAmounts);
    }

    internal void CheckForKey(Door door)
    {
        Debug.Log("Items in EQ " + _inventoryManager.PlayerInventory.ItemsOnGrid.Count);
        foreach(ItemFromInventory item in _inventoryManager.PlayerInventory.ItemsOnGrid)
        {
            if (item.itemData.id == door.KeyID)
            {
                Debug.Log("Using key " + item.ItemName);
                PlayerHover.ShowMessage($"{item.ItemName} worked");
                door.Closed = false;

                item.ItemDescription += " I feel I don't will need it anymore.";
            }
        }
    }

    public void HideInventory()
    {
        if (_inventoryManager.HoldedItem == null)
        {
            _playerControl.SwitchInventory(false);
            if (currentItemStash_Env != null)
            {
                if (currentItemStash_Env.IsStash) _inventoryManager.HideItemStash(currentItemStash_Env);
                if (currentItemStash_Env.IsPlayerStash) _inventoryManager.HidePlayerStash();

                currentItemStash_Env.CloseSprite();
                currentItemStash_Env = null;
            }
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

    void HandleCloseToDoor(Door clickedDoor)
    {
        if(GetPlayerSTATE() == InteractionState.DEFAULT)
            StartCoroutine(CloseToDoor(clickedDoor));
        _playerControl.PlayerMove.MovingToObject = false;
    }

    void HandleCloseToEnvObject(EnvObject clickedObject)
    {
        if (GetPlayerSTATE() == InteractionState.DEFAULT)
            StartCoroutine(CloseToEnvObject(clickedObject));
        _playerControl.PlayerMove.MovingToObject = false;
    }

    void HandleDoor(Door clickedDoor)
    {
        if (!clickedDoor.Closed)
        {
            if (TransferPlayer(clickedDoor))
            {
                RoomManager.ChangeRoom(clickedDoor.Destination.ThisRoom);
            }
        }
        else
        {
            Debug.Log("It's locked!");
            PlayerHover.ShowMessage("It's locked");
            CheckForKey(clickedDoor);
        }
    }

    private void HandleEnvObject(EnvObject clickedObject)
    {
        if (GetPlayerSTATE() == InteractionState.DEFAULT)
        {
            if (clickedObject.IsStash)
            {
                if (clickedObject.InUseSprite != null) clickedObject.gameObject.GetComponent<SpriteRenderer>().sprite = clickedObject.InUseSprite;
                ShowNormalStash(clickedObject);
            }
            if (clickedObject.IsPlayerStash)
            {
                if (clickedObject.InUseSprite != null) clickedObject.gameObject.GetComponent<SpriteRenderer>().sprite = clickedObject.InUseSprite;
                ShowPlayerStash(clickedObject);
            }
        }
    }

    IEnumerator CloseToEnvObject(EnvObject clickedDoor)
    {
        Debug.Log("Getting closer to Object");
        Vector3 EnvDestination = _playerControl.PlayerMove.Agent.destination;
        _playerControl.PlayerMove.MovingToObject = true;
        while (_playerControl.PlayerMove.Agent.remainingDistance > 0.5f)
        {
            if (EnvDestination != _playerControl.PlayerMove.Agent.destination || !_playerControl.PlayerMove.MovingToObject)
            {
                Debug.Log("Break going to object!");
                yield break;
            }

            yield return null;
        }
        HandleEnvObject(clickedDoor);
        yield return null;
    }

    IEnumerator CloseToDoor(Door clickedDoor)
    {
        Debug.Log("Getting closer to Door");
        Vector3 EnvDestination = _playerControl.PlayerMove.Agent.destination;
        while (_playerControl.PlayerMove.Agent.remainingDistance > 0.5f)
        {
            if (EnvDestination != _playerControl.PlayerMove.Agent.destination) yield break;
/*            Debug.Log("Closing... " + Vector3.Distance(_playerControl.transform.position, clickedDoor.transform.position));*/
            yield return null;
        }
        HandleDoor(clickedDoor);
        yield return null;
    } 

}
