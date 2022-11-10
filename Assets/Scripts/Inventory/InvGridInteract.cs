using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryGrid))]
public class InvGridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    InventoryManager inventoryManager;
    InventoryGrid itemGrid;
    Player playerInput;

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.SelectedItemGRID = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!inventoryManager.CheckMouseInInventory())
        {
            inventoryManager.SelectedItemGRID = null;
        }
        
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        itemGrid = GetComponent<InventoryGrid>();
        playerInput = new Player();
        playerInput.Enable();
    }


}
