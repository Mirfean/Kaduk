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
    }

    // Update is called once per frame
    void Update()
    {
        
    }


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


}
