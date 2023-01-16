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

    public static Action<string> OnMouseAboveItem;
    public static Action <string, string> OnLookUpHover;
    public static Action OnMouseLoseFocus;

    [SerializeField] PlayerControl _playerControl;


    // Start is called before the first frame update
    void Start()
    {
        if (DescriptionText == null) HoverWindow.GetComponentInChildren<TextMeshProUGUI>();
        if (_playerControl is null) _playerControl = FindObjectOfType<PlayerControl>();
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

    private void ShowInfo(string name)
    {
        SoloNameText.text = name;
        HoverWindow.sizeDelta = new Vector2(SoloNameText.preferredWidth, SoloNameText.preferredHeight + 50);
        HoverWindow.gameObject.SetActive(true);
        Vector2 hoverNewPos = GetMousePos();
        HoverWindow.transform.position = new Vector2(hoverNewPos.x + 50, hoverNewPos.y + 50);
    }

    private void HideInfo()
    {
        SoloNameText.text = default;
        HoverWindow.gameObject.SetActive(false);
    }

    private void ShowLookUpInfo(string name, string description)
    {
        DescriptionText.text = description;
        NameText.text = name;
        LookUpWindow.sizeDelta = new Vector2(DescriptionText.preferredWidth > 300 ? 300 : DescriptionText.preferredWidth * 2, DescriptionText.preferredHeight + 50);

        LookUpWindow.gameObject.SetActive(true);
        Vector2 hoverNewPos = GetMousePos();
        LookUpWindow.transform.position = new Vector2(hoverNewPos.x + 50, hoverNewPos.y + 50);
    }

    private void HideLookUpInfo()
    {
        NameText.text = default;
        DescriptionText.text = default;
        LookUpWindow.gameObject.SetActive(false);
    }

    public Vector2 GetMousePos()
    {
        Debug.Log("GetMousePos " + _playerControl.PlayerInput.Basic.MouseMovement.ReadValue<Vector2>());
        return _playerControl.PlayerInput.Basic.MouseMovement.ReadValue<Vector2>();
    }
}
