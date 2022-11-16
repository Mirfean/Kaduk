using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryGrid))]
public class InvGridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    InventoryManager _inventoryManager;
    InventoryGrid _itemGrid;
    Player _playerInput;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventoryManager.SelectedItemGRID = _itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_inventoryManager.CheckMouseInInventory())
        {
            _inventoryManager.SelectedItemGRID = null;
        }
        
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _itemGrid = GetComponent<InventoryGrid>();
        _playerInput = new Player();
        _playerInput.Enable();
    }


}
