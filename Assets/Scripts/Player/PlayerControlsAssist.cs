using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerControl))]
public class PlayerControlsAssist
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void MovingThroughInventory(ref Player PlayerInput, ref InventoryManager inventoryManager)
    {
        Debug.Log("Moving through inventory");
        if (inventoryManager.HoldedItem != null)
        {
            if (inventoryManager.CurrentItemRectTransform != null)
            {
                inventoryManager.MoveItemIcon(PlayerInput.UI.MousePosition.ReadValue<Vector2>());
                inventoryManager.HandleHighlight(PlayerInput.UI.MousePosition.ReadValue<Vector2>());
            }
        }
    }
    


}
