using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    ItemGrid itemGrid;

    [SerializeField]
    InventoryItem selectedItem;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void getGridPos(Vector2 mousePos)
    {
        if (itemGrid != null)
        {
            Debug.Log($"Grid {itemGrid.GetInvGridPositon(mousePos)}");
        }
    }

    public void ItemMove(Vector2 mousePos)
    {
        Vector2Int tileGridPosition = itemGRID.GetInvGridPositon(mousePos);
        Debug.Log(tileGridPosition.ToString());

        if (selectedItem == null)
        {
            selectedItem = itemGrid.PickUpItem(tileGridPosition.x, Mathf.Abs(tileGridPosition.y));
        }
        else
        {
            itemGrid.PlaceItem(selectedItem, tileGridPosition.x, Mathf.Abs(tileGridPosition.y));
        }
    }
}
