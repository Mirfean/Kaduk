using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverButton : MonoBehaviour
{
    public HoverButtonEnum ButtonType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ClickButton()
    {
        ClickHoverManager.OnButtonClick(this.gameObject);
    }

    /*
        TAKE_ITEM,
        USE,
        LOOK,
        SPLIT,
        COMBINE,
        DESTROY,
        CANCEL
    */

}
