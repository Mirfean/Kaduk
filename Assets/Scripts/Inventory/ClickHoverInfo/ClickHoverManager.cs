using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickHoverManager : MonoBehaviour
{
    public RectTransform ClickHoverWindow;
    public List<HoverButton> ButtonList;
    private StashType _currentStashType;


    private InventoryManager _inventoryManager;


    public static Action<StashType> OnHoverOpen;
    public static Action <GameObject> OnButtonClick;
    public static Action OnHoverClose;

    private static Player _playerInput;

    private void OnEnable()
    {
        OnButtonClick += ServiceButton;
        OnHoverOpen += PrepareAndShowHover;
        OnHoverClose += HideHover;
    }

    private void OnDisable()
    {
        OnButtonClick -= ServiceButton;
        OnHoverOpen -= PrepareAndShowHover;
        OnHoverClose -= HideHover;
    }

    // Start is called before the first frame update
    void Start()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        ClickHoverWindow.gameObject.SetActive(false);
        ButtonList = new List<HoverButton>();
        foreach(HoverButton HB in ClickHoverWindow.gameObject.GetComponentsInChildren<HoverButton>())
        {
            ButtonList.Add(HB);
        }
        _playerInput = new Player();
        _playerInput.Enable();
    }

    public static Vector2 GetMousePos()
    {
        return _playerInput.Basic.MouseMovement.ReadValue<Vector2>();
    }

    public void PrepareAndShowHover(StashType stashType)
    {
        Debug.Log("Stash type " + stashType);
        _currentStashType = stashType;
        ClickHoverWindow.gameObject.SetActive(true);
        var x = new List<HoverButtonEnum>();
        TextMeshProUGUI cancelText = ButtonList.Find(i => i.ButtonType == HoverButtonEnum.CANCEL).gameObject.GetComponent<TextMeshProUGUI>();
        switch (stashType)
        {
            case StashType.INVENTORY:
                x.Add(HoverButtonEnum.TAKE_ITEM);
                x.Add(HoverButtonEnum.USE);
                x.Add(HoverButtonEnum.LOOK);
                //x.Add(HoverButtonEnum.COMBINE); TO ADD
                //x.Add(HoverButtonEnum.SPLIT); TO ADD
                x.Add(HoverButtonEnum.CANCEL);
                ClickHoverWindow.sizeDelta = new Vector2(cancelText.preferredWidth, 5 + (cancelText.preferredHeight * (x.Count + 2)));
                ShowProperButtons(x);
                break;
            
            case StashType.ITEMSTASH:
                x.Add(HoverButtonEnum.TAKE_ITEM);
                x.Add(HoverButtonEnum.LOOK);
                x.Add(HoverButtonEnum.SPLIT);
                x.Add(HoverButtonEnum.CANCEL);
                ClickHoverWindow.sizeDelta = new Vector2(cancelText.preferredWidth, 5 + (cancelText.preferredHeight * (x.Count + 2)));
                ShowProperButtons(x);
                break;
            
            case StashType.PLAYER_STASH:
                x.Add(HoverButtonEnum.TAKE_ITEM);
                x.Add(HoverButtonEnum.USE);
                x.Add(HoverButtonEnum.LOOK);
                //x.Add(HoverButtonEnum.COMBINE); TO ADD
                //x.Add(HoverButtonEnum.SPLIT); TO ADD
                x.Add(HoverButtonEnum.CANCEL);
                ClickHoverWindow.sizeDelta = new Vector2(cancelText.preferredWidth, 5 + (cancelText.preferredHeight * (x.Count + 2)));
                ShowProperButtons(x);
                break;
        }
        ClickHoverWindow.transform.position = GetMousePos();
    }

    void ShowProperButtons(List<HoverButtonEnum> activeButtons)
    {
        foreach(HoverButton HB in ButtonList)
        {
            if (activeButtons.Contains(HB.ButtonType))
            {
                HB.gameObject.SetActive(true);
            }
            else HB.gameObject.SetActive(false);
        }
    }

    void HideHover()
    {
        ClickHoverWindow.gameObject.SetActive(false);
    }

    public void ServiceButton(GameObject button)
    {
        InventoryManager invManager = FindObjectOfType<InventoryManager>();
        switch (button.GetComponent<HoverButton>().ButtonType)
        {
            case HoverButtonEnum.TAKE_ITEM:
                Debug.Log("TAKE ITEM BUTTON");
                invManager.ChangeGridForItem(_currentStashType);
                break;
            case HoverButtonEnum.USE:
                Debug.Log("USE BUTTON");
                //Healing, etc
                break;
            case HoverButtonEnum.LOOK:
                Debug.Log("LOOK BUTTON");
                //Show full description
                break;
            case HoverButtonEnum.SPLIT:
                // If item is stackable, allow to split it
                break;
            case HoverButtonEnum.COMBINE:
                // If item is combinable, try to combine with next clicked item
                break;
            case HoverButtonEnum.DESTROY:
                // After confirmation delete item
                break;
            case HoverButtonEnum.CANCEL:
                Debug.Log("CANCEL BUTTON");

                // Hide this popup and return to previous state
                break;
        }
        HideHover();
    }
}
