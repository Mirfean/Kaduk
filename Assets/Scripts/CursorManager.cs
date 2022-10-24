using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    CursorType currentCursor;

    [SerializeField]
    Texture2D[] cursors;

    public void Start()
    {
        currentCursor = CursorType.STANDARD;
        ChangeCursorTo(currentCursor);
    }

    public void HideCursorToAim()
    {
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
    }

    public CursorType CurrentCursor
    {
        get { return currentCursor; }
        set 
        { 
            currentCursor = value;
            
        }
    }

    public void ChangeCursorTo(CursorType cursorType)
    {
        int cursorInt = (int) cursorType;
        if(cursorInt <= cursors.Length - 1)
        {
            Cursor.SetCursor(cursors[cursorInt], Vector2.zero, CursorMode.ForceSoftware);
        }
        
    }


}
