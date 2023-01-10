using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverInfoManager : MonoBehaviour
{
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI SoloNameText;
    public RectTransform HoverWindow;
    public RectTransform LookUpWindow;

    public static Action<string, Vector2> OnMouseAboveItem;
    public static Action <string, string, Vector2> OnLookUpHover;
    public static Action OnMouseLoseFocus;

    private static Player _playerInput;


    // Start is called before the first frame update
    void Start()
    {
        if (DescriptionText == null) HoverWindow.GetComponentInChildren<TextMeshProUGUI>();
        _playerInput = FindObjectOfType<PlayerControl>().PlayerInput;
        HideInfo();
    }

    private void OnEnable()
    {
        OnLookUpHover += ShowLookUpInfo;
        OnMouseAboveItem += ShowInfo;
        OnMouseLoseFocus += HideInfo;
    }

    private void OnDisable()
    {
        OnLookUpHover += ShowLookUpInfo;
        OnMouseAboveItem += ShowInfo;
        OnMouseLoseFocus -= HideInfo;
    }

    private void ShowInfo(string name, Vector2 mousePos)
    {
        SoloNameText.text = name;
        HoverWindow.sizeDelta = new Vector2(SoloNameText.preferredWidth, SoloNameText.preferredHeight + 50);
        HoverWindow.gameObject.SetActive(true);
        HoverWindow.transform.position = new Vector2(mousePos.x + 50, mousePos.y + 50);
    }

    private void HideInfo()
    {
        SoloNameText.text = default;
        HoverWindow.gameObject.SetActive(false);
    }

    private void ShowLookUpInfo(string name, string description, Vector2 mousePos)
    {
        DescriptionText.text = description;
        NameText.text = name;
        LookUpWindow.sizeDelta = new Vector2(DescriptionText.preferredWidth > 300 ? 300 : DescriptionText.preferredWidth * 2, DescriptionText.preferredHeight + 50);

        LookUpWindow.gameObject.SetActive(true);
        LookUpWindow.transform.position = new Vector2(mousePos.x + 50, mousePos.y + 50);
    }

    private void HideLookUpInfo()
    {
        NameText.text = default;
        DescriptionText.text = default;
        LookUpWindow.gameObject.SetActive(false);
    }

    public static Vector2 GetMousePos()
    {
        return _playerInput.Basic.MouseMovement.ReadValue<Vector2>();
    }
}
