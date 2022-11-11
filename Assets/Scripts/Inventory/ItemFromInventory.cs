using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


enum Rotation
{
    r0 = 0, // Anchors min(0,1) max(0,1) pivot(0,1)
    r90 = 1, // Anchors min(1,1) max(1,1) pivot(1,1)
    r180 = 2, // Anchors min(1,0) max(1,0) pivot(1,0)
    r270 = 3 // Anchors min(0,0) max(0,0) pivot(0,0)
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

    public bool[,] spaceFill;

    public int onGridPositionX;

    public int onGridPositionY;

    public int HEIGHT
    {
        get
        {
            if (rotation == Rotation.r0 || rotation == Rotation.r180) return itemdata.height;
            else return itemdata.width;
        }

    }

    public int WIDTH
    {
        get
        {
            if (rotation == Rotation.r0 || rotation == Rotation.r180) return itemdata.width;
            else return itemdata.height;
        }
    }

    

    public ItemData itemData
    {
        get { return itemdata; }
        set
        { 
            itemdata = value;

            GetComponent<Image>().sprite = itemdata.itemIcon;

            itemDescription = itemdata.description;

            Vector2 size = new Vector2(WIDTH * InventoryGrid.tileSizeWidth, HEIGHT * InventoryGrid.tileSizeHeight);

            GetComponent<RectTransform>().sizeDelta = size;

            spaceFill = itemdata.fill;
        }
    }

    internal void Set(ItemData itemdata)
    {
        this.itemData = itemdata;
    }

    internal void rotate()
    {
        spaceFill = rotateFill();
        
        if (rotation == Rotation.r270) rotation = Rotation.r0;
        else rotation += 1;

        //if(this.gameObject.transform.rotation.z == 360f) this.gameObject.transform.Rotate(0f, 0f, -360f);
        //else this.gameObject.transform.Rotate(0f, 0f, 90f);

        this.gameObject.transform.Rotate(0f, 0f, 90f);

        ChangePivot();
        
    }

    private bool[,] rotateFill()
    {
        bool[,] newFill = new bool[HEIGHT, WIDTH];
        for (int i = 0; i < WIDTH; i++)
        {
            for(int j = 0; j < HEIGHT; j++)
            {
                Debug.Log($"for new[{j},{i}] value of old[{WIDTH - 1 - i},{ HEIGHT - (HEIGHT - j)}]");
                newFill[j, i] = spaceFill[WIDTH - 1 - i, HEIGHT - (HEIGHT - j)];
            }
        }
        return newFill;
    }

    public void ChangePivot()
    {
        Vector2 newValue = new Vector2();

        switch (rotation)
        {
            case Rotation.r0:
                {
                    newValue = new Vector2(0, 1);
                    break;
                }
            case Rotation.r90:
                {
                    newValue = new Vector2(1, 1);
                    break;
                }

            case Rotation.r180:
                {
                    newValue = new Vector2(1, 0);
                    break;
                }

            case Rotation.r270:
                {
                    newValue = new Vector2(0, 0);
                    
                    break;
                }         

        }

        gameObject.GetComponent<RectTransform>().pivot = newValue;
        gameObject.GetComponent<RectTransform>().anchorMin = newValue;
        gameObject.GetComponent<RectTransform>().anchorMax = newValue;
        gameObject.GetComponent<RectTransform>().anchoredPosition = newValue;
    }
}
