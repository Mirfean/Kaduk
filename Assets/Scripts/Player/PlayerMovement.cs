using UnityEngine;
using UnityEngine.AI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
#if ENABLE_INPUT_SYSTEM
    [SerializeField]
    private InputActionAsset _inputActions;
    private InputActionMap _playerActionMap;
    private InputAction _movement;
#endif
    [SerializeField]
    private Camera Camera;
    private NavMeshAgent Agent;
    [SerializeField]
    [Range(0, 0.99f)]
    private float Smoothing = 0.25f;
    [SerializeField]
    private float TargetLerpSpeed = 1;

    private Vector3 TargetDirection;
    private float LerpTime = 0;
    private Vector3 LastDirection;
    private Vector3 MovementVector;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        _playerActionMap = _inputActions.FindActionMap("Basic");
        _movement = _playerActionMap.FindAction("WSAD");
        _movement.started += HandleMovementAction;
        _movement.canceled += HandleMovementAction;
        _movement.performed += HandleMovementAction;
        _movement.Enable();
        _playerActionMap.Enable();
        _inputActions.Enable();

    }

    private void HandleMovementAction(InputAction.CallbackContext Context)
    {
        Vector2 input = Context.ReadValue<Vector2>();
        MovementVector = new Vector3(input.x, input.y, 0);
    }


    private void Update()
    {
        DoNewInputSystemMovement();
    }
    private void DoNewInputSystemMovement()
    {
        MovementVector.Normalize();
        if (MovementVector != LastDirection)
        {
            LerpTime = 0;
        }
        LastDirection = MovementVector;
        TargetDirection = Vector3.Lerp(
            TargetDirection,
            MovementVector,
            Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
        );

        Agent.Move(TargetDirection * Agent.speed * Time.deltaTime);

        /*Vector3 lookDirection = MovementVector;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(lookDirection),
                Mathf.Clamp01(LerpTime * TargetLerpSpeed * (1 - Smoothing))
            );
        }*/

        LerpTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        //Camera.transform.position = transform.position + Vector3.up * 10;
    }
}
