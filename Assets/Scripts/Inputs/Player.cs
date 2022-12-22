//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Scripts/Inputs/Player.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Player : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Player()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player"",
    ""maps"": [
        {
            ""name"": ""Basic"",
            ""id"": ""4bacb287-a1ab-4d01-b762-023e8163386f"",
            ""actions"": [
                {
                    ""name"": ""WSAD"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0fccd3a2-53a7-4dce-89cd-133a099c5b0a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MouseMovement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f73a3bb8-cc8c-43c2-a6aa-91ff9e26fa29"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseLClick"",
                    ""type"": ""Button"",
                    ""id"": ""93f80fac-3167-4e81-96e0-c1df76887163"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""eae3dc8f-1f9c-4d9c-9409-e34dbcbe788f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""a4cca925-4621-4a3f-acf9-eeb8ba30b9a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Flashlight"",
                    ""type"": ""Button"",
                    ""id"": ""ce2bb281-1c6f-4a3d-a0c7-65dc463fefa9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""0d6d2a9b-fb55-4cfa-bd8d-69aecc6a32d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""e95daeec-98e9-4eab-aa3c-5da9fa207291"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WSAD"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4c36a52f-add0-49d2-9ffa-7ea37a1762e1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WSAD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ed6acf12-d818-4d88-9227-3ffce3b5416e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WSAD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cb0fc4af-35e3-4c42-8370-26f1430777f5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WSAD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ffcd116c-119d-4524-8589-e6fe7db66376"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WSAD"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e61ec01e-cda3-414f-bbb7-731b2f021175"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4c436ba0-e2d5-4095-bbe7-1831aece8823"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53398251-4ccd-4f9a-84ab-c33652871a41"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41c9698d-8002-41e7-9e3a-4f58cc9693ce"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a09c6936-6140-499a-b12a-2a6a74cc031f"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Flashlight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1233437d-b52b-46db-955c-d0eb846c651e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""73c6e039-0501-4a7b-9624-1639d5c7825d"",
            ""actions"": [
                {
                    ""name"": ""MouseLClick"",
                    ""type"": ""Button"",
                    ""id"": ""a06a9703-e173-46a4-90bd-acb2a6218cca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a64c46f4-7cd3-4b88-a5e4-c2d019c819b0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InventoryClose"",
                    ""type"": ""Button"",
                    ""id"": ""7c5a67b0-03da-4e23-bc11-4f89120eebc5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpawnItem"",
                    ""type"": ""Button"",
                    ""id"": ""845ed7a2-ed85-4eab-b16b-bda9505a636b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InsertItem"",
                    ""type"": ""Button"",
                    ""id"": ""a977cfc2-0fd7-4b44-9638-35aeda945f81"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RotateItem"",
                    ""type"": ""Button"",
                    ""id"": ""fd1eeb0e-4e46-406b-96cb-2bdaa46a39e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MouseRClick"",
                    ""type"": ""Button"",
                    ""id"": ""254855a3-3359-4a48-907f-858ffec3edd1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fc905580-c687-4d5f-9f2f-b27ff03d63c9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseLClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eee7c5aa-032e-4619-a849-c555d78864a4"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed98ce75-f831-44ff-9dd7-e699992a5581"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InventoryClose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e1a6fbd-b4f2-440f-8b22-8a0c42cc5c02"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9399468f-e707-45c2-8f61-3bf3fe4cac7c"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InsertItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b0a6c14-c5f6-4305-905c-41fa8a45ce29"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4ce3b3d-f4c2-456f-94dc-f797bd43d45e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseRClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Basic
        m_Basic = asset.FindActionMap("Basic", throwIfNotFound: true);
        m_Basic_WSAD = m_Basic.FindAction("WSAD", throwIfNotFound: true);
        m_Basic_MouseMovement = m_Basic.FindAction("MouseMovement", throwIfNotFound: true);
        m_Basic_MouseLClick = m_Basic.FindAction("MouseLClick", throwIfNotFound: true);
        m_Basic_Aim = m_Basic.FindAction("Aim", throwIfNotFound: true);
        m_Basic_Inventory = m_Basic.FindAction("Inventory", throwIfNotFound: true);
        m_Basic_Flashlight = m_Basic.FindAction("Flashlight", throwIfNotFound: true);
        m_Basic_Interact = m_Basic.FindAction("Interact", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_MouseLClick = m_UI.FindAction("MouseLClick", throwIfNotFound: true);
        m_UI_MousePosition = m_UI.FindAction("MousePosition", throwIfNotFound: true);
        m_UI_InventoryClose = m_UI.FindAction("InventoryClose", throwIfNotFound: true);
        m_UI_SpawnItem = m_UI.FindAction("SpawnItem", throwIfNotFound: true);
        m_UI_InsertItem = m_UI.FindAction("InsertItem", throwIfNotFound: true);
        m_UI_RotateItem = m_UI.FindAction("RotateItem", throwIfNotFound: true);
        m_UI_MouseRClick = m_UI.FindAction("MouseRClick", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Basic
    private readonly InputActionMap m_Basic;
    private IBasicActions m_BasicActionsCallbackInterface;
    private readonly InputAction m_Basic_WSAD;
    private readonly InputAction m_Basic_MouseMovement;
    private readonly InputAction m_Basic_MouseLClick;
    private readonly InputAction m_Basic_Aim;
    private readonly InputAction m_Basic_Inventory;
    private readonly InputAction m_Basic_Flashlight;
    private readonly InputAction m_Basic_Interact;
    public struct BasicActions
    {
        private @Player m_Wrapper;
        public BasicActions(@Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @WSAD => m_Wrapper.m_Basic_WSAD;
        public InputAction @MouseMovement => m_Wrapper.m_Basic_MouseMovement;
        public InputAction @MouseLClick => m_Wrapper.m_Basic_MouseLClick;
        public InputAction @Aim => m_Wrapper.m_Basic_Aim;
        public InputAction @Inventory => m_Wrapper.m_Basic_Inventory;
        public InputAction @Flashlight => m_Wrapper.m_Basic_Flashlight;
        public InputAction @Interact => m_Wrapper.m_Basic_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Basic; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BasicActions set) { return set.Get(); }
        public void SetCallbacks(IBasicActions instance)
        {
            if (m_Wrapper.m_BasicActionsCallbackInterface != null)
            {
                @WSAD.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnWSAD;
                @WSAD.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnWSAD;
                @WSAD.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnWSAD;
                @MouseMovement.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseMovement;
                @MouseLClick.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseLClick;
                @MouseLClick.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseLClick;
                @MouseLClick.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnMouseLClick;
                @Aim.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnAim;
                @Inventory.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnInventory;
                @Flashlight.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnFlashlight;
                @Flashlight.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnFlashlight;
                @Flashlight.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnFlashlight;
                @Interact.started -= m_Wrapper.m_BasicActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_BasicActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_BasicActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_BasicActionsCallbackInterface = instance;
            if (instance != null)
            {
                @WSAD.started += instance.OnWSAD;
                @WSAD.performed += instance.OnWSAD;
                @WSAD.canceled += instance.OnWSAD;
                @MouseMovement.started += instance.OnMouseMovement;
                @MouseMovement.performed += instance.OnMouseMovement;
                @MouseMovement.canceled += instance.OnMouseMovement;
                @MouseLClick.started += instance.OnMouseLClick;
                @MouseLClick.performed += instance.OnMouseLClick;
                @MouseLClick.canceled += instance.OnMouseLClick;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @Flashlight.started += instance.OnFlashlight;
                @Flashlight.performed += instance.OnFlashlight;
                @Flashlight.canceled += instance.OnFlashlight;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public BasicActions @Basic => new BasicActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_MouseLClick;
    private readonly InputAction m_UI_MousePosition;
    private readonly InputAction m_UI_InventoryClose;
    private readonly InputAction m_UI_SpawnItem;
    private readonly InputAction m_UI_InsertItem;
    private readonly InputAction m_UI_RotateItem;
    private readonly InputAction m_UI_MouseRClick;
    public struct UIActions
    {
        private @Player m_Wrapper;
        public UIActions(@Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseLClick => m_Wrapper.m_UI_MouseLClick;
        public InputAction @MousePosition => m_Wrapper.m_UI_MousePosition;
        public InputAction @InventoryClose => m_Wrapper.m_UI_InventoryClose;
        public InputAction @SpawnItem => m_Wrapper.m_UI_SpawnItem;
        public InputAction @InsertItem => m_Wrapper.m_UI_InsertItem;
        public InputAction @RotateItem => m_Wrapper.m_UI_RotateItem;
        public InputAction @MouseRClick => m_Wrapper.m_UI_MouseRClick;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @MouseLClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseLClick;
                @MouseLClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseLClick;
                @MouseLClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseLClick;
                @MousePosition.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMousePosition;
                @InventoryClose.started -= m_Wrapper.m_UIActionsCallbackInterface.OnInventoryClose;
                @InventoryClose.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnInventoryClose;
                @InventoryClose.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnInventoryClose;
                @SpawnItem.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSpawnItem;
                @SpawnItem.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSpawnItem;
                @SpawnItem.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSpawnItem;
                @InsertItem.started -= m_Wrapper.m_UIActionsCallbackInterface.OnInsertItem;
                @InsertItem.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnInsertItem;
                @InsertItem.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnInsertItem;
                @RotateItem.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRotateItem;
                @RotateItem.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRotateItem;
                @RotateItem.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRotateItem;
                @MouseRClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseRClick;
                @MouseRClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseRClick;
                @MouseRClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMouseRClick;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseLClick.started += instance.OnMouseLClick;
                @MouseLClick.performed += instance.OnMouseLClick;
                @MouseLClick.canceled += instance.OnMouseLClick;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @InventoryClose.started += instance.OnInventoryClose;
                @InventoryClose.performed += instance.OnInventoryClose;
                @InventoryClose.canceled += instance.OnInventoryClose;
                @SpawnItem.started += instance.OnSpawnItem;
                @SpawnItem.performed += instance.OnSpawnItem;
                @SpawnItem.canceled += instance.OnSpawnItem;
                @InsertItem.started += instance.OnInsertItem;
                @InsertItem.performed += instance.OnInsertItem;
                @InsertItem.canceled += instance.OnInsertItem;
                @RotateItem.started += instance.OnRotateItem;
                @RotateItem.performed += instance.OnRotateItem;
                @RotateItem.canceled += instance.OnRotateItem;
                @MouseRClick.started += instance.OnMouseRClick;
                @MouseRClick.performed += instance.OnMouseRClick;
                @MouseRClick.canceled += instance.OnMouseRClick;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IBasicActions
    {
        void OnWSAD(InputAction.CallbackContext context);
        void OnMouseMovement(InputAction.CallbackContext context);
        void OnMouseLClick(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnFlashlight(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnMouseLClick(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnInventoryClose(InputAction.CallbackContext context);
        void OnSpawnItem(InputAction.CallbackContext context);
        void OnInsertItem(InputAction.CallbackContext context);
        void OnRotateItem(InputAction.CallbackContext context);
        void OnMouseRClick(InputAction.CallbackContext context);
    }
}
