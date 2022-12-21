using Assets.Scripts.Enums;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerBasics : MonoBehaviour
{
    private Player _playerInput;

    [SerializeField]
    float _defaultSpeed = 3f;

    [SerializeField]
    private float _speed = 3f;

/*    [SerializeField]
    private bool _isAiming = false;

    public bool IsDialogue = false;

    public bool IsInventory = false;*/

    [SerializeField]
    InteractionState _state = InteractionState.DEFAULT;

    public InteractionState STATE
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

    [SerializeField]
    Rigidbody2D _rigidbody;

    PlayerMovement _playerMovement;

    public PlayerMovement PlayerMove { get { return _playerMovement; } }

    [SerializeField]
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = new Player();
        _playerInput.Enable();
        //selector = GetComponent<Selector>();
        _inventoryManager = FindObjectOfType<InventoryManager>();

        _rigidbody = GetComponent<Rigidbody2D>();

        _playerMovement = GetComponent<PlayerMovement>();

        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

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

    public Vector2 GetMousePos()
    {
        return _playerInput.Basic.MouseMovement.ReadValue<Vector2>();
    }

    /// <summary>
    /// Used by PlayerInput in Player.Basic.MouseLClick
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseClick(InputAction.CallbackContext context)
    {

        if (STATE == InteractionState.INVENTORY && context.phase == InputActionPhase.Started)
        {
            Debug.Log("Inventory click");
            _inventoryManager.GrabAndDropItemIcon(_playerInput.UI.MousePosition.ReadValue<Vector2>());
            return;
        }
        else if (STATE == InteractionState.DIALOGUE)
        {
            Debug.Log("Dialogue click");
            return;
        }

        else if (STATE == InteractionState.AIMING)
        {
            Debug.Log("Shooting click");
            if (_weapon != null && !_playerInput.Basic.MouseLClick.inProgress && context.phase == InputActionPhase.Started)
            {
                Debug.Log("Piu piu");
                _weapon.GetComponent<_Weapon>().Attack(Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>()), _skeletanMove.HoldedItem.rotation);
            }
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(_playerInput.Basic.MouseMovement.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out hit) && STATE == InteractionState.DEFAULT)
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

        if (!_playerInput.Basic.MouseLClick.inProgress && context.phase == InputActionPhase.Started && STATE == InteractionState.DEFAULT)
        {
            Vector2 target = Camera.main.ScreenToWorldPoint(_playerInput.Basic.MouseMovement.ReadValue<Vector2>());
            Debug.Log(target);

            _playerMovement.MouseMovement(target);
        }

        
        
    }

    /// <summary>
    /// Actions when mouse is moving
    /// </summary>
    /// <param name="context"></param>
    public void OnMovementMouse(InputAction.CallbackContext context)
    {
        if (STATE == InteractionState.INVENTORY)
        {
            
            /*if (_inventoryManager.SelectedItemGRID && _inventoryManager.CheckMouseInInventory())*/
            if(_inventoryManager.SelectedItem != null)
            {
                if (_inventoryManager.CurrentItemRectTransform != null)
                {
                    _inventoryManager.MoveItemIcon(_playerInput.UI.MousePosition.ReadValue<Vector2>());
                    _inventoryManager.HandleHighlight(_playerInput.UI.MousePosition.ReadValue<Vector2>());
                }
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

            if (STATE == InteractionState.AIMING)
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


            _rigidbody.MovePosition(Vector2.MoveTowards(transform.position, new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, 0), _speed * Time.deltaTime));
            //transform.Translate(direction * Time.deltaTime * _speed);

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
            Debug.Log(InteractionState.INVENTORY);
            yield return null;
        } while (_playerInput.Basic.Aim.IsInProgress());
        ChangeAimStatus(false);
        _speed = _defaultSpeed;
        Debug.Log("stop aiming");
        yield return null;
    }

    void ChangeAimStatus(bool status)
    {
        if (status) STATE = InteractionState.AIMING;
        else STATE = InteractionState.DEFAULT;

        _animator.SetBool("IsAiming", status);
        if (STATE != InteractionState.AIMING) {
            _cursorManager.ShowCursor();
            _weapon.gameObject.SetActive(false);
            _skeletanMove.SetArmsToIdle();
        }
        if (STATE == InteractionState.AIMING)
        {
            StopMoveCoroutines();
            _cursorManager.HideCursorToAim();
            _weapon.gameObject.SetActive(true);
        }
    }

    void RotateWeapon()
    {
        _weapon.GetComponent<Gun>().RotateGun(Rotated);
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

    //Only for testing
    public void SpawnItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(_inventoryManager.ItemPrefab).GetComponent<ItemFromInventory>();
        _inventoryManager.SelectedItem = item;

        _inventoryManager.CurrentItemRectTransform = item.GetComponent<RectTransform>();
        _inventoryManager.CurrentItemRectTransform.SetParent(_inventoryManager.InventoryCanvasTransform);

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

