using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InventoryGrid))]
public class InvGridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    InventoryManager _inventoryManager;
    [SerializeField]
    InventoryGrid _itemGrid;

    // Start is called before the first frame update
    void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _itemGrid = GetComponent<InventoryGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventoryManager.SelectedItemGRID = _itemGrid;
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Checking");
        if (!_inventoryManager.CheckMouseInInventory())
        {
            _inventoryManager.SelectedItemGRID = null;
        }

    }




}
