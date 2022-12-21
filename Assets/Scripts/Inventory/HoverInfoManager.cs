using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverInfoManager : MonoBehaviour
{
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI NameText;
    public RectTransform TipWindow;

    public static Action <string, string, Vector2> OnMouseHover;
    public static Action OnMouseLoseFocus;

    private static Player _playerInput;


    // Start is called before the first frame update
    void Start()
    {
        if (DescriptionText == null) TipWindow.GetComponentInChildren<TextMeshProUGUI>();
        _playerInput = new Player();
        _playerInput.Enable();
        HideTip();
    }

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    private void ShowTip(string name, string description, Vector2 mousePos)
    {
        DescriptionText.text = description;
        NameText.text = name;
        TipWindow.sizeDelta = new Vector2(DescriptionText.preferredWidth > 300 ? 300: DescriptionText.preferredWidth * 2, DescriptionText.preferredHeight + 50);

        TipWindow.gameObject.SetActive(true);
        TipWindow.transform.position = new Vector2(mousePos.x + 50, mousePos.y + 50);
    }

    private void HideTip()
    {
        DescriptionText.text = default;
        TipWindow.gameObject.SetActive(false);
    }

    public static Vector2 GetMousePos()
    {
        return _playerInput.Basic.MouseMovement.ReadValue<Vector2>();
    }
}
