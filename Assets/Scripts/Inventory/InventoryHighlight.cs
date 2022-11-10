using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField]
    internal RectTransform highlighter;

    public void Show(bool v)
    {
        highlighter.gameObject.SetActive(v);
    }

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

    public void SetPosition(InventoryGrid targetGrid, ItemFromInventory targetItem, int posX, int posY)
    {
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2 pos = targetGrid.GetItemPosition(targetItem, posX, posY);

        Debug.Log("SetPos " + posX + " " + posY);

        highlighter.localPosition = pos;
    }

    public void SetParent(InventoryGrid targetGrid)
    {
        if (targetGrid == null) return;
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

   
}
