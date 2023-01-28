using Assets.Scripts;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField]
    CursorType _currentCursor;

    [SerializeField]
    Texture2D[] _cursors;

    public void Start()
    {
        _currentCursor = CursorType.STANDARD;
        ChangeCursorTo(_currentCursor);
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
        get { return _currentCursor; }
        set
        {
            _currentCursor = value;

        }
    }

    public void ChangeCursorTo(CursorType cursorType)
    {
        int cursorInt = (int)cursorType;
        if (cursorInt <= _cursors.Length - 1)
        {
            Cursor.SetCursor(_cursors[cursorInt], Vector2.zero, CursorMode.ForceSoftware);
        }

    }


}
