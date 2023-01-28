using UnityEngine;

/// <summary>
/// Class necessary for Dialogue System with new Input
/// </summary>
public class InputRegistration : MonoBehaviour
{
#if USE_NEW_INPUT

    // Track which instance of this script registered the inputs, to prevent
    // another instance from accidentally unregistering them.
    protected static bool isRegistered = false;
    private bool _didIRegister = false;

    private void Awake()
    {

    }

    /*    void OnEnable()
        {
            if (!isRegistered)
            {
                isRegistered = true;
                _didIRegister = true;
                //UserInput.Instance.Input.Enable();
                InputDeviceManager.RegisterInputAction("WSAD", UserInput.Instance.Input.Basic.WSAD);
                InputDeviceManager.RegisterInputAction("MouseMovement", UserInput.Instance.Input.Basic.MouseMovement);
                InputDeviceManager.RegisterInputAction("MouseLClick", UserInput.Instance.Input.Basic.MouseLClick);
                InputDeviceManager.RegisterInputAction("Aim", UserInput.Instance.Input.Basic.Aim);
            }
        }

        void OnDisable()
        {
            if (_didIRegister)
            {
                isRegistered = false;
                _didIRegister = false;
                //UserInput.Instance.Input.Disable();
                InputDeviceManager.UnregisterInputAction("WSAD");
                InputDeviceManager.UnregisterInputAction("MouseMovement");
                InputDeviceManager.UnregisterInputAction("MouseLClick");
                InputDeviceManager.UnregisterInputAction("Aim");
            }
        }*/

#endif

}
