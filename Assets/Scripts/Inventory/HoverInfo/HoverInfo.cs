using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string ItemDescription;
    public string ItemName;
    private float _timeToWait = 0.5f;
    [SerializeField] private bool _isStatusInfo;

    public void Start()
    {
        if (!_isStatusInfo)
        {
            ItemName = GetComponent<ItemFromInventory>().itemData.ItemName;
            ItemDescription = GetComponent<ItemFromInventory>().itemData.Description;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        if (!_isStatusInfo)
        {
            InventoryManager.OnMouseAboveItem(GetComponent<ItemFromInventory>());
        }
        StartCoroutine(StartTimer());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        HoverInfoManager.OnMouseLoseFocus();
    }

    private void ShowMessage()
    {
        if (_isStatusInfo)
        {
            HoverInfoManager.OnMouseAboveItem(ItemDescription);
        }
        else
        {
            HoverInfoManager.OnMouseAboveItem(ItemName);
        }

    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(_timeToWait);

        ShowMessage();
    }


}
