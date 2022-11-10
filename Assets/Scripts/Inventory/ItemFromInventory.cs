using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


enum Rotation
{
    r0 = 0,
    r90 = 1,
    r180 = 2,
    r270 = 3
}

/// <summary>
/// Created item in inventory
/// </summary>
public class ItemFromInventory : MonoBehaviour
{
    [SerializeField]
    public string itemDescription;
    ItemData itemdata;

    Rotation rotation = Rotation.r0;

    public int onGridPositionX;

    public int onGridPositionY;

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

    internal void Set(ItemData itemdata)
    {
        this.itemData = itemdata;
    }

    internal void rotate(bool clockwise)
    {
        if (clockwise)
        {
            rotation += 1;
            this.gameObject.transform.Rotate(0f, 90f, 0f);
        }
        else
        {
            rotation -= 1;
            this.gameObject.transform.Rotate(0f, -90f, 0f);
        }
    }

    internal void rotateFill(bool clockwise)
    {
        if (clockwise)
        {
            bool[,] newFill = new bool[itemData.height, itemData.width];
            for (int i = 0; i < itemData.height; i++)
            { }
            
        }
        else
        {

        }
    }
}
