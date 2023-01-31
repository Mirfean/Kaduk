using System;
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
    public static Action<string, string> OnLookUpHover;
    public static Action OnMouseLoseFocus;


    // Start is called before the first frame update
    void Start()
    {
        if (DescriptionText == null) HoverWindow.GetComponentInChildren<TextMeshProUGUI>();
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
        OnLookUpHover -= ShowLookUpInfo;
        OnMouseAboveItem -= ShowInfo;
        OnMouseLoseFocus -= HideInfo;
    }

    private void ShowInfo(string name)
    {
        SoloNameText.text = name;
        HoverWindow.sizeDelta = new Vector2(SoloNameText.preferredWidth * 1.5f, SoloNameText.preferredHeight + 50);

        HoverWindow.gameObject.SetActive(true);

        Vector2 hoverNewPos = UserInput.Instance.GetUIMousePos();
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

        LookUpWindow.sizeDelta = new Vector2(DescriptionText.preferredWidth > 300 ? 300 : DescriptionText.preferredWidth * 2.5f, DescriptionText.preferredHeight + 10);
        LookUpWindow.gameObject.SetActive(true);

        Vector2 hoverNewPos = UserInput.Instance.GetUIMousePos();
        LookUpWindow.transform.position = new Vector2(hoverNewPos.x + 30, hoverNewPos.y + 30);
    }

    private void HideLookUpInfo()
    {
        NameText.text = default;
        DescriptionText.text = default;
        LookUpWindow.gameObject.SetActive(false);
    }
}
