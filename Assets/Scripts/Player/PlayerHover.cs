using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHover : MonoBehaviour
{
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

        SpeechBubble.gameObject.SetActive(true);

        StartCoroutine(SpeechBubbleTimer(2f));
    }

    IEnumerator SpeechBubbleTimer(float timer)
    {
        yield return new WaitForSeconds(timer);

        if(SpeechBubble.gameObject.activeSelf) SpeechBubble.gameObject.SetActive(false);
        DialogueText.text = "";
    }


}
