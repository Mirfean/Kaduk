using System;
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
    [SerializeField]
    private float _targetLerpSpeed = 1;

    private Vector3 _targetDirection;
    private float _lerpTime = 0;
    private Vector3 _lastDirection;
    private Vector3 _movementVector;

    private bool _isMoving;

    [SerializeField] float _defaultSpeed = 3f;
    [SerializeField] float _speed = 3f;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
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
        if (_movement.inProgress && _movementVector != Vector3.zero) WsadMovement();
        else if (_movementVector == Vector3.zero && _isMoving)
        {
            _agent.ResetPath();
            _isMoving = false;
        }
    }
    private void WsadMovement()
    {
        if (!_isMoving) _isMoving = true;
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

        if (_agent.SetDestination(transform.position + _targetDirection))
        {
            Debug.Log("WsadMovement");
        }

        _agent.Move(_targetDirection * _agent.speed * Time.deltaTime);

        _lerpTime += Time.deltaTime;
    }

    internal void MouseMovement(Vector2 target)
    {
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
}
