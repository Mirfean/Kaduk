using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryGrid))]
public class InvGridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    InventoryManager inventoryManager;
    InventoryGrid itemGrid;

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.itemGRID = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //inventoryManager.itemGRID = null;
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        itemGrid = GetComponent<InventoryGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
