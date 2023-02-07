using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerNavMeshMovement : MonoBehaviour
{
#if ENABLE_INPUT_SYSTEM
    [SerializeField]
    private InputActionAsset _inputActions;
    private InputActionMap _playerActionMap;
    private InputAction _movement;
#endif
    [SerializeField]
    private Camera Camera;
    private NavMeshAgent _agent;
    public NavMeshAgent Agent { get { return _agent; } }

    public bool IsMoving { get => _isMoving; set { _isMoving = value; PlayerControl.WalkModeChange(value); } }

    [SerializeField]
    private float _targetLerpSpeed = 1;

    private Vector3 _targetDirection;
    private float _lerpTime = 0;
    private Vector3 _lastDirection;
    private Vector3 _movementVector;

    private bool _isMoving;

    [SerializeField] float _defaultSpeed = 3f;
    [SerializeField] float _speed = 3f;

    PlayerControl _playerControl;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _playerControl = GetComponent<PlayerControl>();
        _playerActionMap = _inputActions.FindActionMap("Basic");
        _movement = _playerActionMap.FindAction("WSAD");
        _movement.Enable();
        _playerActionMap.Enable();
        _inputActions.Enable();

        _agent.updatePosition = true;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    public void HandleMovementAction(InputAction.CallbackContext Context)
    {
        Vector2 input = Context.ReadValue<Vector2>();
        _movementVector = new Vector3(input.x, input.y, 0);
    }


    private void Update()
    {
        if (_movement.inProgress &&
            _movementVector != Vector3.zero &&
            (_playerControl.STATE == InteractionState.DEFAULT || _playerControl.STATE == InteractionState.AIMING)) WsadMovement();
        else if (_movementVector == Vector3.zero && IsMoving)
        {
            _agent.ResetPath();
            //Add action to change IsMoving
            IsMoving = false;
        }

    }
    private void WsadMovement()
    {
        if (!IsMoving) IsMoving = true;
        _movementVector.Normalize();

        //Workaround on issue with moving UP/DOWN
        _movementVector.x = _movementVector.x == 0.0f ? 0.001f : _movementVector.x;

        if (_movementVector != _lastDirection)
        {
            _lerpTime = 0;
        }
        _lastDirection = _movementVector;

        _targetDirection = Vector3.Lerp(_targetDirection, _movementVector, Mathf.Clamp01(_lerpTime * _targetLerpSpeed));
        _agent.ResetPath();

        if (_playerControl.STATE == InteractionState.DEFAULT)
            _agent.Move(_targetDirection * _agent.speed * Time.deltaTime);
        else if (_playerControl.STATE == InteractionState.AIMING)
            _agent.Move(_targetDirection * _agent.speed / 3 * Time.deltaTime);

        _lerpTime += Time.deltaTime;
    }

    internal void MouseMovement(Vector2 target)
    {
        Debug.Log("new Path!");
        _agent.ResetPath();
        _agent.SetDestination(target);
    }

    public void ModifySpeed(bool v)
    {
        if (v)
        {
            _speed = _speed / 3;
        }
        else
        {
            _speed = _defaultSpeed;
        }
    }

    public void StopWhenClose(Transform ObjectOfInterest)
    {
        StartCoroutine(StopClose(ObjectOfInterest));
    }

    IEnumerator StopClose(Transform ObjectOfInterest)
    {
        while (Agent.pathPending)
        {
            Debug.Log("Pending path");
            yield return null;
        }

        Debug.Log("Remains " + _agent.remainingDistance);
        while (_agent.remainingDistance > 0.5f)
        {
            //Debug.Log("Closing... " + _agent.remainingDistance);
            yield return null;
        }
        _agent.ResetPath();
        yield return null;
    }

    bool CheckIfClose(Vector3 OOI, float howClose)
    {
        return Mathf.Abs(_agent.transform.position.x - OOI.x) <= howClose && Mathf.Abs(_agent.transform.position.y - OOI.y) <= howClose;
    }
}
