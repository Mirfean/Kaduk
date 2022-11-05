using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField]
    RectTransform highlighter;

    public void SetSize(ItemFromInventory targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * InventoryGrid.tileSizeWidth;
        size.y = targetItem.itemData.height * InventoryGrid.tileSizeHeight;
        highlighter.sizeDelta = size;
    }

    public void SetPosition(InventoryGrid targetGrid, ItemFromInventory targetItem)
    {
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2 pos = targetGrid.GetItemPosition(targetItem, targetItem.onGridPositionX, targetItem.onGridPositionY);
    
        highlighter.localPosition = pos;
    }


   
}
