using Assets.Scripts.Enums;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasics : MonoBehaviour
{
    private Player _playerInput;

    [SerializeField]
    float _defaultSpeed = 3f;

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private bool _isAiming = false;

    public bool IsDialogue = false;

    public bool IsInventory = false;

    [SerializeField]
    InteractionState _state;

    InteractionState STATE
    {
        get { return _state; }
        set { _state = value; }
    }

    [SerializeField]
    private bool _shootInProgress = false;

    [SerializeField]
    private bool _rotated = false;

    private bool Rotated
    {
        get { return _rotated; }
        set
        {
            if (_weapon.gameObject.activeSelf && _rotated != value)
            {
                RotateWeapon();
            }
            _rotated = value;
            
        }
    }

    [SerializeField]
    _Weapon _weapon;

    [SerializeField]
    private Texture2D _crosshair;

    //private List<Coroutine> moveCoroutineList;
    [SerializeField]
    private Coroutine _moveCoroutine;

    [SerializeField]
    private SkeletalMove _skeletanMove;

    [SerializeField]
    Animator _animator;

    [SerializeField]
    Transform _characterSprite;

    [SerializeField]
    CursorManager _cursorManager;

    [SerializeField]
    InventoryManager _inventoryManager;



    // Start is called before the first frame update
    void Start()
    {
        _playerInput = new Player();
        _playerInput.Enable();
        //selector = GetComponent<Selector>();
        _inventoryManager = FindObjectOfType<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //For Testing
        /*if(inventoryManager.itemGRID != null)
        {
            if (playerInput.UI.enabled)  inventoryManager.getGridPos(playerInput.UI.MousePosition.ReadValue<Vector2>());
            else inventoryManager.getGridPos(playerInput.Basic.MouseMovement.ReadValue<Vector2>());

        }*/
    }

    public void TurnOffInput()
    {
        StopMoveCoroutines();
        _playerInput.Disable();
    }

    public void TurnOnInput()
    {
        _playerInput.Enable();
    }

    public void ChangeIsUsing(bool state)
    {
        IsDialogue = state;
    }

    /// <summary>
    /// Used by PlayerInput in Player.Basic.MouseLClick
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseClick(InputAction.CallbackContext context)
    {

        if (IsInventory)
        {
            _inventoryManager.GrabAndDropItemIcon(_playerInput.UI.MousePosition.ReadValue<Vector2>());
            return;
        }
        else if (IsDialogue)
        {
            //TODO
            return;
        }

        _shootInProgress = _playerInput.Basic.MouseLClick.inProgress;
        Debug.Log("TWOJA STARA " + _shootInProgress);
        if (_isAiming)
        {
            if(_weapon != null && !_shootInProgress && context.phase == InputActionPhase.Started)
            {
                Debug.Log("Piu piu");
                _weapon.GetComponent<_Weapon>().Attack(Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>()), _skeletanMove.HoldedItem.rotation);
            }
        }

        else
        {
            StopMoveCoroutines();
            Coroutine co = StartCoroutine(MovingByClick(context));
            _moveCoroutine = co;
        }
        
    }

    IEnumerator MovingByClick(InputAction.CallbackContext context)
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>());
        _animator.SetBool("Walk", true);
        do
        {
            transform.position = Vector2.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            //Debug.Log((transform.position.x - target.x) + " " + (transform.position.y - target.y));
            yield return null;
        } while ((Mathf.Abs(transform.position.x - target.x) > 0.1f || Mathf.Abs(transform.position.y - target.y) > 0.1f));
        
        Debug.Log("Ending movement by mouse");
        StopAllCoroutines();
        yield return null;
        
    }

    /// <summary>
    /// Actions when mouse is moving
    /// </summary>
    /// <param name="context"></param>
    public void OnMovementMouse(InputAction.CallbackContext context)
    {
        if (IsInventory)
        {
            if (_inventoryManager.SelectedItemGRID && _inventoryManager.CheckMouseInInventory())
            {
                _inventoryManager.MoveItemIcon(_playerInput.UI.MousePosition.ReadValue<Vector2>());
                _inventoryManager.HandleHighlight(_playerInput.UI.MousePosition.ReadValue<Vector2>());
                return;
            }
        }
        if (_playerInput.Basic.enabled)
        {
            Vector2 realPos = Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>());
            _skeletanMove.RotateFlashlight(realPos);
            if (realPos.x < transform.position.x)
            {
                _characterSprite.rotation = Quaternion.Euler(0, 180, 0);
                if(!Rotated) Rotated = true;
            }
            else if (_rotated)
            {
                _characterSprite.rotation = Quaternion.Euler(0, 0, 0);
                if (Rotated) Rotated = false;
            }

            if (_isAiming)
            {
                Debug.Log("Aim");
                Aiming(realPos);
                //CorrectPistolToLeftHand();
            }
        }
        

    }

    private void Aiming(Vector2 realPos)
    {
        _skeletanMove.TrackCursorByHands(realPos);
        _weapon.transform.position = _skeletanMove.LeftHand.position;

    }

    /// <summary>
    /// INACTIVE
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    IEnumerator AimWeapon(InputAction.CallbackContext context)
    {
        _skeletanMove.TrackCursorByHands(Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>()));
        yield return null;
    }

    /// <summary>
    /// Character movement by WSAD
    /// Run Coroutine CharacterMovementWSAD
    /// </summary>
    /// <param name="context"></param>
    public void OnMovementWSAD(InputAction.CallbackContext context)
    {
        if (IsDialogue) return;
        //Debug.Log(context.valueType);
        Debug.Log(context.ReadValue<Vector2>());
        //Debug.Log(context.ReadValue<Vector2>() + " Vector?");
        StopMoveCoroutines();
        Coroutine co = StartCoroutine(CharacterMovementWSAD(context));
        _moveCoroutine = co;
    }

    /// <summary>
    /// Coroutine to move character by WSAD
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    IEnumerator CharacterMovementWSAD(InputAction.CallbackContext context)
    {
        _animator.SetBool("Walk", true);
        do
        {
            Vector2 direction = reduceDiagonallyMovement(context.ReadValue<Vector2>());

            
            transform.Translate(direction * Time.deltaTime * _speed);
            //yield return new WaitForSeconds(0.05f);
            yield return null;

        } while (_playerInput.Basic.WSAD.phase.IsInProgress());
        _animator.SetBool("Walk", false);
        StopMoveCoroutines();

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

    void StopMoveCoroutines()
    {
        _animator.SetBool("Walk", false);
        if (_moveCoroutine != null)
        {
            Debug.Log("Killing movement by mouse");
            StopCoroutine(_moveCoroutine);
        }
        
        
    }


    /// <summary>
    /// RIGHT MOUSE CLICK METHOD
    /// Aiming weapon to cursor position
    /// </summary>
    public void OnAim(InputAction.CallbackContext context)
    {
        Debug.Log("Aiming");
        StartCoroutine(AimCoroutine());
    }

    IEnumerator AimCoroutine()
    {
        _speed = _speed / 3;
        ChangeAimStatus(true);
        //Aiming(Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>()));
        Aiming(Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>()));
        do
        {
            Debug.Log(_isAiming);
            yield return null;
        } while (_playerInput.Basic.Aim.IsInProgress());
        ChangeAimStatus(false);
        _speed = _defaultSpeed;
        Debug.Log("stop aiming");
        yield return null;
    }

    void ChangeAimStatus(bool status)
    {
        _isAiming = status;
        
        _animator.SetBool("IsAiming", _isAiming);
        if (!_isAiming) {
            _cursorManager.ShowCursor();
            _weapon.gameObject.SetActive(false);
            _skeletanMove.SetArmsToIdle();
        }
        if (_isAiming)
        {
            StopMoveCoroutines();
            _cursorManager.HideCursorToAim();
            _weapon.gameObject.SetActive(true);
        }
    }

    void CorrectPistolToLeftHand()
    {
        
        if (_rotated && !_weapon.GetComponent<_Weapon>().Rotated)
        {
            //weapon.transform.Rotate(180f, 180f, 0f);
            Debug.Log("I should rotate weapon");
            //weapon.GetComponent<Gun>().RotateGun(true);
        }
        else if(!_rotated)
        {
            if (_weapon.GetComponent<_Weapon>().Rotated)
            {
                //weapon.GetComponent<Gun>().RotateGun(false);
                //weapon.transform.Rotate(-180f, -180f, 0f);
            }
            
        }
    }

    void RotateWeapon()
    {
        _weapon.GetComponent<Gun>().RotateGun(Rotated);
    }

    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.started) SwitchInventory();
    }

    void SwitchInventory()
    {
        if (_playerInput.Basic.enabled)
        {
            IsInventory = true;
            _inventoryManager.SelectedItemGRID.gameObject.SetActive(true);
        }
        else
        {
            IsInventory = false;
            _inventoryManager.SelectedItemGRID.gameObject.SetActive(false);
        }

        if (IsInventory)
        {
            _playerInput.UI.Enable();
            _playerInput.Basic.Disable();
            Debug.Log("Enabling UI");
        }
        else
        {
            _playerInput.Basic.Enable();
            _playerInput.UI.Disable();
            Debug.Log("Disabling UI");
        }
    }

    public void SpawnItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(_inventoryManager.ItemPrefab).GetComponent<ItemFromInventory>();
        _inventoryManager.SelectedItem = item;

        _inventoryManager.CurrentItemRectTransform = item.GetComponent<RectTransform>();
        _inventoryManager.CurrentItemRectTransform.SetParent(_inventoryManager.CanvasTransform);

        int selectedItemID = Random.Range(0, _inventoryManager.ItemsList.Count - 1);
        //item.itemData = 
        _inventoryManager.SelectedItem.itemData = _inventoryManager.ItemsList[selectedItemID];

        Debug.Log($"Spawn {_inventoryManager.SelectedItem}");
    }

    public void FlashlightONOFF(InputAction.CallbackContext context)
    {
        _skeletanMove.ChangeFlashlightMode();
    } 
}

