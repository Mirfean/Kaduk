using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created item in inventory
/// </summary>
public class ItemFromInventory : MonoBehaviour
{
    [SerializeField]
    public string itemDescription;
    ItemData itemdata;
    public ItemData itemData
    {
        get { return itemdata; }
        set
        { 
            itemdata = value;

            GetComponent<Image>().sprite = itemdata.itemIcon;

            itemDescription = itemdata.description;

            Vector2 size = new Vector2(itemData.width * InventoryGrid.tileSizeWidth, itemData.height * InventoryGrid.tileSizeHeight);

            GetComponent<RectTransform>().sizeDelta = size;

        }
    }

    public int onGridPositionX;

    public int onGridPositionY;



    internal void Set(ItemData itemdata)
    {
        this.itemData = itemdata;
    }
}
