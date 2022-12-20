using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField]
    public RectTransform HighlighterRectTrans;

    public void Show(bool v)
    {
        HighlighterRectTrans.gameObject.SetActive(v);
    }

    public void SetSize(ItemFromInventory targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * InventoryGrid.TileSizeWidth;
        size.y = targetItem.HEIGHT * InventoryGrid.TileSizeHeight;
        HighlighterRectTrans.sizeDelta = size;
    }

    public void SetPosition(InventoryGrid targetGrid, ItemFromInventory targetItem)
    {
        HighlighterRectTrans.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2 pos = targetGrid.GetItemPosition(targetItem, targetItem.OnGridPositionX, targetItem.OnGridPositionY);
            
        HighlighterRectTrans.localPosition = pos;
    }

    public void SetPosition(InventoryGrid targetGrid, ItemFromInventory targetItem, int posX, int posY)
    {
        HighlighterRectTrans.SetParent(targetGrid.GetComponent<RectTransform>());

        Vector2 pos = targetGrid.GetItemPosition(targetItem, posX, posY);

        //Debug.Log("SetPos " + posX + " " + posY);

        HighlighterRectTrans.localPosition = pos;
    }

    public void SetParent(InventoryGrid targetGrid)
    {
        if (targetGrid == null) return;
        HighlighterRectTrans.SetParent(targetGrid.GetComponent<RectTransform>());
    }

   
}
