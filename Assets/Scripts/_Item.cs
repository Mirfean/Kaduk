using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Item : MonoBehaviour
{
    [SerializeField]
    private CursorManager _cursorManager;

    [SerializeField]
    public string Description;

    // Start is called before the first frame update
    void Start()
    {
        _cursorManager = FindObjectOfType<CursorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnUse(Transform player)
    {
        Debug.Log("KWA");
    }

    private void OnMouseEnter()
    {
        _cursorManager.ChangeCursorTo(CursorType.EYE);
    }

    private void OnMouseExit()
    {
        //TODO execute method 
        _cursorManager.ChangeCursorTo(CursorType.STANDARD);
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"{Description}");
    }
}
