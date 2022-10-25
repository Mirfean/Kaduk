using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers;

/// <summary>
/// Class necessary for Dialogue System with new Input
/// </summary>
public class InputRegistration : MonoBehaviour
{
#if USE_NEW_INPUT

    private Player playerInput;


    // Track which instance of this script registered the inputs, to prevent
    // another instance from accidentally unregistering them.
    protected static bool isRegistered = false;
    private bool didIRegister = false;

    void Awake()
    {
        playerInput = new Player();
    }

    void OnEnable()
    {
        if (!isRegistered)
        {
            isRegistered = true;
            didIRegister = true;
            playerInput.Enable();
            InputDeviceManager.RegisterInputAction("WSAD", playerInput.Basic.WSAD);
            InputDeviceManager.RegisterInputAction("MouseMovement", playerInput.Basic.MouseMovement);
            InputDeviceManager.RegisterInputAction("MouseLClick", playerInput.Basic.MouseLClick);
            InputDeviceManager.RegisterInputAction("Aim", playerInput.Basic.Aim);
        }
    }

    void OnDisable()
    {
        if (didIRegister)
        {
            isRegistered = false;
            didIRegister = false;
            playerInput.Disable();
            InputDeviceManager.UnregisterInputAction("WSAD");
            InputDeviceManager.UnregisterInputAction("MouseMovement");
            InputDeviceManager.UnregisterInputAction("MouseLClick");
            InputDeviceManager.UnregisterInputAction("Aim");
        }
    }

#endif

}
