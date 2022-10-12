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
    GameObject weapon;

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



    // Start is called before the first frame update
    void Start()
    {
        playerInput = new Player();
        playerInput.Enable();

        //playerInput.Basic.WSAD.started += OnMovement;
        
        //playerInput.Basic.WSAD.performed += OnMovement;

        ChangeCursorToCrossHair();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playerInput.Basic.WSAD.phase.IsInProgress())
        {
            OnMovement();
        }*/


    }

    public void OnMouseClick(InputAction.CallbackContext context)
    {
        shootInProgress = playerInput.Basic.MouseLClick.inProgress;
        if (IsAiming)
        {
            if(weapon != null && !shootInProgress)
            {
                Debug.Log("Piu piu");
                skeletanMove.TrackCursorByHands(Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>()));
                //weapon.GetComponent<_Weapon>().Attack(Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>()));
                //weapon.GetComponent<_Weapon>().Attack(playerInput.Basic.MouseMovement.ReadValue<Vector2>());
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
    /// INACTIVE
    /// </summary>
    /// <param name="context"></param>
    public void OnMovementMouse(InputAction.CallbackContext context)
    {
        Vector2 realPos = Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>());
        if (realPos.x < transform.position.x)
        {
            CharacterSprite.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            CharacterSprite.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (IsAiming)
        {
            Debug.Log("Aim");
            skeletanMove.TrackCursorByHands(realPos);
            CorrectPistolToLeftHand();
            //StartCoroutine(AimWeapon(context));
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
        do
        {
            Vector2 direction = reduceDiagonallyMovement(context.ReadValue<Vector2>());

            transform.Translate(direction * Time.deltaTime * speed);
            //yield return new WaitForSeconds(0.05f);
            yield return null;

        } while (playerInput.Basic.WSAD.phase.IsInProgress());
        

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
        if (moveCoroutine != null)
        {
            Debug.Log("Killing movement by mouse");
            StopCoroutine(moveCoroutine);
        }
        
        
    }

    void ChangeCursorToCrossHair()
    {
        Cursor.SetCursor(crosshair, Vector2.zero, CursorMode.ForceSoftware);
    }

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
        if(!IsAiming) skeletanMove.SetArmsToIdle();
    }

    void CorrectPistolToLeftHand()
    {
        weapon.transform.position = skeletanMove.leftHand.position;
    }

}

