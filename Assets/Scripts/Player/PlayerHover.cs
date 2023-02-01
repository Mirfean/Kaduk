using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHover : MonoBehaviour
{
    Color _fullColor = new Color(0, 0, 0, 255);

    public RectTransform SpeechBubble;
    public TextMeshProUGUI DialogueText;
    public Transform point;

    public static Action<string> ShowMessage;

    void OnEnable()
    {
        ShowMessage += ShowSpeechBubble;
    }

    private void OnDisable()
    {
        ShowMessage -= ShowSpeechBubble;
    }

    private void Start()
    {

    }

    void Update()
    {
        
    }

    void ShowSpeechBubble(string textToShow)
    {
        DialogueText.text = textToShow;

        //SpeechBubble.sizeDelta = new Vector2(10, 5);

        //SpeechBubble.transform.position = Camera.main.WorldToScreenPoint(point.position);

        ActivateBubble();

        StartCoroutine(SpeechBubbleTimer(2f));
    }

    IEnumerator SpeechBubbleTimer(float timer)
    {
        yield return new WaitForSeconds(timer/2);
        SpeechBubble.GetComponent<Image>().CrossFadeAlpha(0, timer / 2, false);
        DialogueText.CrossFadeAlpha(0, timer / 2, false);
        
        yield return new WaitForSeconds(timer / 2);
        
        SpeechBubble.gameObject.SetActive(false);
        DialogueText.gameObject.SetActive(false);
        
        DialogueText.text = "";
        yield return null;
    }


    void ActivateBubble()
    {
        SpeechBubble.gameObject.SetActive(true);
        SpeechBubble.GetComponent<Image>().color = _fullColor;
        
        DialogueText.gameObject.SetActive(true);
    }

}
