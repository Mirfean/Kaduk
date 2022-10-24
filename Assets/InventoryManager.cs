using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    ItemGrid itemGrid;

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
}
