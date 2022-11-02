using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class InvGridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    InventoryManager inventoryManager;
    ItemGrid itemGrid;

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
        itemGrid = GetComponent<ItemGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
