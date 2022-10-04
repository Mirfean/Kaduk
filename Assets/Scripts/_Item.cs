using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Item : MonoBehaviour
{
    [SerializeField]
    private Texture2D eyeCursor;

    [SerializeField]
    public string description;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(eyeCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        //TODO execute method 
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log($"{description}");
    }
}
