using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string ItemDescription;
    public string ItemName;
    private float _timeToWait = 0.5f;

    public void Start()
    {
        ItemName = GetComponent<ItemFromInventory>().itemData.name;
        ItemDescription = GetComponent<ItemFromInventory>().itemData.Description;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        InventoryManager.OnMouseAboveItem(GetComponent<ItemFromInventory>());
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverInfoManager.OnMouseLoseFocus();
    }

    private void ShowMessage()
    {

        Debug.Log("Mouse POS " + HoverInfoManager.GetMousePos());
        HoverInfoManager.OnMouseHover(ItemName, ItemDescription, HoverInfoManager.GetMousePos());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_timeToWait);

        ShowMessage();
    }


}
