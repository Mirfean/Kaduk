using Assets.Scripts.Enums;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerBasics : MonoBehaviour
{
    Player _playerInput;

    public Player PlayerInput { get => _playerInput; set => _playerInput = value; }

    [SerializeField] InteractionState _state = InteractionState.DEFAULT;
    public InteractionState STATE
    {
        get { return _state; }
        set { _state = value; }
    }

    [SerializeField] PlayerWeapon _playerWeapon;

    [SerializeField] CursorManager _cursorManager;

    [SerializeField] InventoryManager _inventoryManager;

    [SerializeField] Coroutine _moveCoroutine;

    [SerializeField] SkeletalMove _skeletalMove;

    [SerializeField] Animator _animator;

    [SerializeField] Transform _characterSprite;

    [SerializeField] PlayerNavMeshMovement _playerMovement;
    public PlayerNavMeshMovement PlayerMove { get { return _playerMovement; } }

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private PlayerControl _playerControl;


    Action<bool> AimModeChange;

    public static Action<bool> WalkModeChange;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput = new Player();
        PlayerInput.Enable();

        _playerControl = new PlayerControl();

        _playerWeapon = GetComponent<PlayerWeapon>();
        _skeletalMove = GetComponent<SkeletalMove>();
        _playerMovement = GetComponent<PlayerNavMeshMovement>();
        
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _gameManager = FindObjectOfType<GameManager>();
        
        AimModeChange += ChangeAimStatus;
        AimModeChange += _playerMovement.ModifySpeed;

        WalkModeChange += ChangeWalking;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Basic " + PlayerInput.Basic.enabled);
        Debug.Log("UI " + PlayerInput.UI.enabled);
    }

    public void TurnOffInput()
    {
        StopMoveCoroutines();
        PlayerInput.Disable();
    }

    public void TurnOnInput()
    {
        PlayerInput.Enable();
    }

    public Vector2 GetMousePos()
    {
        return PlayerInput.Basic.MouseMovement.ReadValue<Vector2>();
    }

    #region Input
    public void OnMouseClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            switch (STATE)
            {
                case InteractionState.AIMING:
                    Debug.Log("Weapon attack");
                    _playerWeapon.UseWeapon(_skeletalMove, PlayerInput);
                    return;
                case InteractionState.DEFAULT:
                    ClickOnDefault();
                    return;
                case InteractionState.DIALOGUE:
                    Debug.Log("Dialogue click");
                    return;
                case InteractionState.INVENTORY:
                    Debug.Log("Inventory click");
                    _inventoryManager.GrabAndDropItemIcon(PlayerInput.UI.MousePosition.ReadValue<Vector2>());
                    return;
            }
        }
    }

    public void OnMovementMouse(InputAction.CallbackContext context)
    {
        if (PlayerInput == null) return;
        if (STATE == InteractionState.INVENTORY)
        {
            _playerControl.MovingThroughInventory(ref _playerInput, ref _inventoryManager);
        }
        if (STATE == InteractionState.DEFAULT)
        {

        }
        Vector2 realPos = Camera.main.ScreenToWorldPoint(PlayerInput.Basic.MouseMovement.ReadValue<Vector2>());
        if (PlayerInput.Basic.enabled)
        {
            _skeletalMove.RotateFlashlight(realPos);
            CharacterRotation(realPos);

            if (STATE == InteractionState.AIMING)
            {
                Debug.Log("Aim");
                if(_playerWeapon.CurrentWeapon.weaponType == WeaponType.HANDGUN)
                {
                    PistolAiming(realPos);
                }
                
            }
        }


    }

    #endregion

    #region Movement
    void StopMoveCoroutines()
    {
        _animator.SetBool(AnimVariable.Walk, false);
        if (_moveCoroutine != null)
        {
            Debug.Log("Killing movement by mouse");
            StopCoroutine(_moveCoroutine);
        }


    }

    /// <summary>
    /// Method reducing double speed when going diagonally
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    Vector2 reduceDiagonallyMovement(Vector2 vector2)
    {
        float checkValue(float v)
        {
            return v is > 0 and < 1 ? 0.5f : v;
        }

        return new Vector2(checkValue(vector2.x), checkValue(vector2.y));
    }

    #endregion

    #region Aiming

    /// <summary>
    /// RIGHT MOUSE CLICK METHOD
    /// Aiming weapon to cursor position
    /// </summary>
    public void OnAim(InputAction.CallbackContext context)
    {
        if (STATE == InteractionState.DEFAULT)
        {
            StartCoroutine(AimCoroutine());
        }

    }

    //MUST STAY HERE :<
    internal IEnumerator AimCoroutine()
    {
        //Aiming(Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>()));
        AimModeChange(true);
        if(_playerWeapon.IsItGun()) PistolAiming(Camera.main.ScreenToWorldPoint(PlayerInput.Basic.MouseMovement.ReadValue<Vector2>()));
        if (_playerWeapon.CurrentWeapon.weaponType == WeaponType.KNIFE) KnifeInHand();
        do
        {
            _playerWeapon.AttachKnife(arm: _skeletalMove.RightArm, hand: _skeletalMove.RightHand);
            Debug.Log("Aiming !!! !!!");
            yield return null;
        } while (PlayerInput.Basic.Aim.IsInProgress());
        Debug.Log("stop aiming");
        AimModeChange(false);
        yield return null;
    }

    internal void PistolAiming(Vector2 realPos)
    {
        _skeletalMove.TrackCursorByHands(realPos);
        _playerWeapon.CurrentWeapon.transform.position = _skeletalMove.LeftHand.position;
    }

    internal void KnifeInHand()
    {
        _playerWeapon.CurrentWeapon.transform.position = _skeletalMove.RightHand.position;
    }

    void ChangeAimStatus(bool status)
    {
        if (status) STATE = InteractionState.AIMING;
        else STATE = InteractionState.DEFAULT;

        //Change Animator variables
        _animator.SetBool(AnimVariable.IsAiming, status);
        if (_playerWeapon.IsItGun()) _animator.SetBool(AnimVariable.RangedWeapon, status);
        else _animator.SetBool(AnimVariable.MeleeWeapon, status);

        if (STATE != InteractionState.AIMING)
        {
            _cursorManager.ShowCursor();
            _playerWeapon.HideWeapon();
            if(_playerWeapon.IsItGun()) _skeletalMove.SetArmsToIdle();
            Debug.Log("Siema z Change Aim 2");
        }
        if (STATE == InteractionState.AIMING)
        {
            StopMoveCoroutines();
            _cursorManager.HideCursorToAim();
            _playerWeapon.ShowWeapon();
            Debug.Log("Siema z Change Aim");
        }
    }

    #endregion

    #region Inventory

    public void InventoryRightClick(InputAction.CallbackContext context)
    {
        Debug.Log("Right click");
        if (context.phase == InputActionPhase.Started &&
            STATE == InteractionState.INVENTORY &&
            _inventoryManager.OnMouseItem != null)
        {
            Debug.Log("Right click right");
            _inventoryManager.ClickedItem = _inventoryManager.OnMouseItem;
            ClickHoverManager.OnHoverOpen(_inventoryManager.ClickedItem.transform.parent.GetComponent<InventoryGrid>().stashType);
        }
    }

    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.started && STATE == InteractionState.INVENTORY) SwitchInventory(false);
        else SwitchInventory(true);
    }

    public void SwitchInventory(bool mode)
    {
        Debug.Log("Switch inventory");
        if (mode)
        {
            STATE = InteractionState.INVENTORY;
            _inventoryManager.ShowInventory();
        }
        else
        {
            STATE = InteractionState.DEFAULT;
            _inventoryManager.HideInventory();
        }

        if (STATE == InteractionState.INVENTORY)
        {
            PlayerInput.UI.Enable();
            PlayerInput.Basic.Disable();
            Debug.Log("Enabling UI");
        }
        else
        {
            PlayerInput.Basic.Enable();
            PlayerInput.UI.Disable();
            Debug.Log("Disabling UI");
        }
    }

    //Only for testing
    public void SpawnItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(_inventoryManager.ItemPrefab).GetComponent<ItemFromInventory>();
        _inventoryManager.HoldedItem = item;

        _inventoryManager.CurrentItemRectTransform = item.GetComponent<RectTransform>();
        _inventoryManager.CurrentItemRectTransform.SetParent(_inventoryManager.InventoryCanvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, _inventoryManager.ItemsList.Count - 1);
        //item.itemData = 
        _inventoryManager.HoldedItem.itemData = _inventoryManager.ItemsList[selectedItemID];

        Debug.Log($"Spawn {_inventoryManager.HoldedItem}");
    }

    #endregion

    #region Rotation of Character and Weapon
    private void CharacterRotation(Vector2 realPos)
    {
        if (realPos.x < transform.position.x)
        {
            RotateCharacterAndWeapon(true);
        }
        else if (_playerWeapon.Rotated)
        {
            RotateCharacterAndWeapon(false);
        }
    }

    private void RotateCharacterAndWeapon(bool reversed)
    {
        if (reversed)
        {
            _characterSprite.rotation = Quaternion.Euler(0, 180, 0);
            _playerWeapon.Rotated = true;
        }
        else
        {
            _characterSprite.rotation = Quaternion.Euler(0, 0, 0);
            _playerWeapon.Rotated = false;
        }
        
    }
    #endregion

    private void ClickOnDefault()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(PlayerInput.Basic.MouseMovement.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("clicked something");

            //Debug.DrawLine(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>()), Color.cyan, 10.0f);
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Debug.Log("I clicked " + objectHit.gameObject.name);
                if (objectHit != null && objectHit.gameObject.GetComponent<Door>() != null)
                {
                    Debug.Log("Door clicked");
                    _gameManager.TransferPlayer(objectHit.gameObject.GetComponent<Door>());
                }

            }
        }

        if (!PlayerInput.Basic.MouseLClick.inProgress)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(PlayerInput.Basic.MouseMovement.ReadValue<Vector2>());
            Debug.Log(target);
            _playerMovement.MouseMovement(target);
        }
    }
    public void FlashlightONOFF(InputAction.CallbackContext context)
    {
        _skeletalMove.ChangeFlashlightMode();
    }

    private void ChangeWalking(bool status)
    {
        _animator.SetBool(AnimVariable.Walk, status);
    }



}

