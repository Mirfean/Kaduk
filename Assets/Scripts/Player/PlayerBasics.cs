using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBasics : MonoBehaviour
{
    private Player playerInput;

    [SerializeField]
    float defaultSpeed = 3f;

    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private bool isMoving = false;

    [SerializeField]
    private bool IsAiming = false;

    [SerializeField]
    private bool shootInProgress = false;

    [SerializeField]
    private bool rotated = false;

    [SerializeField]
    _Weapon weapon;

    [SerializeField]
    private Texture2D crosshair;

    //private List<Coroutine> moveCoroutineList;
    [SerializeField]
    private Coroutine moveCoroutine;

    [SerializeField]
    private SkeletalMove skeletanMove;

    [SerializeField]
    Animator animator;

    [SerializeField]
    Transform CharacterSprite;

    [SerializeField]
    CursorManager cursorManager;

    
    public bool IsDialogue = false;

    public bool IsInventory = false;

    [SerializeField]
    Selector selector;

    [SerializeField]
    InventoryManager inventoryManager;



    // Start is called before the first frame update
    void Start()
    {
        playerInput = new Player();
        playerInput.Enable();
        selector = GetComponent<Selector>();
        inventoryManager = FindObjectOfType<InventoryManager>();
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
        playerInput.Disable();
    }

    public void TurnOnInput()
    {
        playerInput.Enable();
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
            inventoryManager.GrabAndDropItemIcon(playerInput.UI.MousePosition.ReadValue<Vector2>());
            return;
        }
        else if (IsDialogue)
        {
            //TODO
            return;
        }
        shootInProgress = playerInput.Basic.MouseLClick.inProgress;
        Debug.Log("TWOJA STARA " + shootInProgress);
        if (IsAiming)
        {
            if(weapon != null && !shootInProgress)
            {
                Debug.Log("Piu piu");
                weapon.GetComponent<_Weapon>().Attack(Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>()), skeletanMove.HoldedItem.rotation);
            }
        }
        else
        {
            StopMoveCoroutines();
            Coroutine co = StartCoroutine(MovingByClick(context));
            moveCoroutine = co;
        }
        
    }

    IEnumerator MovingByClick(InputAction.CallbackContext context)
    {
        Vector2 target = Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>());
        animator.SetBool("Walk", true);
        do
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
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
            if (inventoryManager.itemGRID && inventoryManager.CheckMouseInInventory())
            {
                inventoryManager.MoveItemIcon(playerInput.UI.MousePosition.ReadValue<Vector2>());
                inventoryManager.HandleHighlight(playerInput.UI.MousePosition.ReadValue<Vector2>());
                return;
            }
        }
        if (playerInput.Basic.enabled)
        {
            Vector2 realPos = Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>());
            if (realPos.x < transform.position.x)
            {
                CharacterSprite.rotation = Quaternion.Euler(0, 180, 0);
                rotated = true;
            }
            else if (rotated)
            {
                CharacterSprite.rotation = Quaternion.Euler(0, 0, 0);
                rotated = false;
            }

            if (IsAiming)
            {
                Debug.Log("Aim");
                skeletanMove.TrackCursorByHands(realPos);
                CorrectPistolToLeftHand();
            }
        }
        

    } 

    /// <summary>
    /// INACTIVE
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    IEnumerator AimWeapon(InputAction.CallbackContext context)
    {
        skeletanMove.TrackCursorByHands(Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>()));
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
        moveCoroutine = co;
    }

    /// <summary>
    /// Coroutine to move character by WSAD
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    IEnumerator CharacterMovementWSAD(InputAction.CallbackContext context)
    {
        animator.SetBool("Walk", true);
        do
        {
            Vector2 direction = reduceDiagonallyMovement(context.ReadValue<Vector2>());

            
            transform.Translate(direction * Time.deltaTime * speed);
            //yield return new WaitForSeconds(0.05f);
            yield return null;

        } while (playerInput.Basic.WSAD.phase.IsInProgress());
        animator.SetBool("Walk", false);
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
        animator.SetBool("Walk", false);
        if (moveCoroutine != null)
        {
            Debug.Log("Killing movement by mouse");
            StopCoroutine(moveCoroutine);
        }
        
        
    }


    /// <summary>
    /// CURSOR
    /// </summary>
    public void OnAim(InputAction.CallbackContext context)
    {
        Debug.Log("Aiming");
        StartCoroutine(AimCoroutine());
    }

    IEnumerator AimCoroutine()
    {
        speed = speed / 3;
        ChangeAimStatus(true);
        do
        {
            Debug.Log(IsAiming);
            yield return null;

        } while (playerInput.Basic.Aim.IsInProgress());
        ChangeAimStatus(false);
        speed = defaultSpeed;
        Debug.Log("stop aiming");
        yield return null;
    }

    void ChangeAimStatus(bool status)
    {
        IsAiming = status;
        
        animator.SetBool("IsAiming", IsAiming);
        if (!IsAiming) {
            cursorManager.ShowCursor();
            weapon.gameObject.SetActive(false);
            skeletanMove.SetArmsToIdle();
        }
        if (IsAiming)
        {
            StopMoveCoroutines();
            cursorManager.HideCursorToAim();
            weapon.gameObject.SetActive(true);
        }
    }

    void CorrectPistolToLeftHand()
    {
        weapon.transform.position = skeletanMove.leftHand.position;
        if (rotated && !weapon.GetComponent<_Weapon>().rotated)
        {
            //weapon.transform.Rotate(180f, 180f, 0f);
            weapon.GetComponent<Gun>().RotateGun(true);
        }
        else if(!rotated)
        {
            if (weapon.GetComponent<_Weapon>().rotated)
            {
                weapon.GetComponent<Gun>().RotateGun(false);
                //weapon.transform.Rotate(-180f, -180f, 0f);
            }
            
        }
    }

    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.started) SwitchInventory();
    }

    void SwitchInventory()
    {
        if (playerInput.Basic.enabled)
        {
            IsInventory = true;
        }
        else
        {
            IsInventory = false;
        }

        if (IsInventory)
        {
            playerInput.UI.Enable();
            playerInput.Basic.Disable();
            Debug.Log("Enabling UI");
        }
        else
        {
            playerInput.Basic.Enable();
            playerInput.UI.Disable();
            Debug.Log("Disabling UI");
        }
    }

    public void SpawnItem(InputAction.CallbackContext context)
    {
        ItemFromInventory item = Instantiate(inventoryManager.itemPrefab).GetComponent<ItemFromInventory>();
        inventoryManager.selectedItem = item;

        inventoryManager.currentItemRectTransform = item.GetComponent<RectTransform>();
        inventoryManager.currentItemRectTransform.SetParent(inventoryManager.canvasTransform);

        int selectedItemID = Random.Range(0, inventoryManager.itemsList.Count - 1);
        //item.itemData = 
        inventoryManager.selectedItem.itemData = inventoryManager.itemsList[selectedItemID];

        Debug.Log($"Spawn {inventoryManager.selectedItem}");
    }

}

