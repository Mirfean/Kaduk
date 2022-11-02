using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Core of Inventory
/// Handle input
/// </summary>
public class InventoryManager : MonoBehaviour
{
    Player playerInput;

    [SerializeField]
    ItemGrid itemGrid;

    [SerializeField] public List<ItemData> itemsList;
    [SerializeField] public GameObject itemPrefab;
    [SerializeField] public Transform canvasTransform;

    [SerializeField]
    public ItemFromInventory selectedItem;
    [SerializeField]
    ItemFromInventory overlapItem;

    public ItemGrid itemGRID {
        get
        {
            return itemGrid;
        }
        set
        {
            itemGrid = value;
        }
    }

    [SerializeField]
    public RectTransform currentItemRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new Player();
        playerInput.Enable();
        playerInput.UI.SpawnItem.performed += ctx => SpawnItem(ctx);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Possibly to remove
    /// </summary>
    /// <param name="mousePos"></param>
    public void getGridPos(Vector2 mousePos)
    {
        if (itemGrid != null)
        {
            Debug.Log($"Grid {itemGrid.GetInvGridPositon(mousePos)}");
        }
    }

    /// <summary>
    /// Method executed by PlayerInput MouseLClick (in reality not)
    /// </summary>
    /// <param name="mousePos"></param>
    public void GrabAndDropItemIcon(Vector2 mousePos)
    {
        if (itemGrid != null)
        {
            Vector2Int tileGridPosition = itemGRID.GetInvGridPositon(mousePos);
            Debug.Log(tileGridPosition.ToString());

            if (selectedItem == null)
            {
                GrabItemIcon(tileGridPosition);

            }
            else
            {
                DropItemIcon(tileGridPosition);
            }
        }
    }

    private void GrabItemIcon(Vector2Int tileGridPosition)
    {
        selectedItem = itemGrid.PickUpItem(tileGridPosition.x, Mathf.Abs(tileGridPosition.y));
        if (selectedItem != null)
        {
            currentItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void DropItemIcon(Vector2Int tileGridPosition)
    {
        bool dropComplete= itemGrid.PlaceItem(selectedItem, tileGridPosition.x, Mathf.Abs(tileGridPosition.y), ref overlapItem);
        if (dropComplete)
        {
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                currentItemRectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
        
    }

    /// <summary>
    /// Move clicked item icon across inventory following cursor
    /// </summary>
    /// <param name="mousePos"></param>
    public void MoveItemIcon(Vector2 mousePos)
    {
        if (selectedItem != null)
        {
            currentItemRectTransform.position = mousePos;
        }
    }

    public void SpawnItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(itemPrefab).GetComponent<ItemFromInventory>();
        selectedItem = item;
        
        currentItemRectTransform = item.GetComponent<RectTransform>();
        currentItemRectTransform.SetParent(canvasTransform);

        int selectedItemID = Random.Range(0, itemsList.Count);
        //item.itemData = 
        selectedItem.itemData = itemsList[selectedItemID];

        Debug.Log($"Spawn {selectedItem}");
    }
}
