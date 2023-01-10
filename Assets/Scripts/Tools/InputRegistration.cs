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

    private Player _playerInput;


    // Track which instance of this script registered the inputs, to prevent
    // another instance from accidentally unregistering them.
    protected static bool isRegistered = false;
    private bool _didIRegister = false;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerControl>().PlayerInput;
    }

    void OnEnable()
    {
        if (!isRegistered)
        {
            isRegistered = true;
            _didIRegister = true;
            _playerInput.Enable();
            InputDeviceManager.RegisterInputAction("WSAD", _playerInput.Basic.WSAD);
            InputDeviceManager.RegisterInputAction("MouseMovement", _playerInput.Basic.MouseMovement);
            InputDeviceManager.RegisterInputAction("MouseLClick", _playerInput.Basic.MouseLClick);
            InputDeviceManager.RegisterInputAction("Aim", _playerInput.Basic.Aim);
        }
    }

    void OnDisable()
    {
        if (_didIRegister)
        {
            isRegistered = false;
            _didIRegister = false;
            _playerInput.Disable();
            InputDeviceManager.UnregisterInputAction("WSAD");
            InputDeviceManager.UnregisterInputAction("MouseMovement");
            InputDeviceManager.UnregisterInputAction("MouseLClick");
            InputDeviceManager.UnregisterInputAction("Aim");
        }
    }

#endif

}
