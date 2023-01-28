using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] int destPoint = 0;

    [SerializeField] bool active;
    [SerializeField] bool hunt;

    private NavMeshAgent _agent;

    [SerializeField] private NavMeshAgent _player;

    public bool Active { get => active; set => active = value; }

    public bool debugFirstTime;
    public List<GameObject> PathBalls;
    public GameObject Ballprefab;

    [SerializeField] Transform _spot;


    [SerializeField] float _attackReload;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
        _agent.updatePosition = true;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {

    }

    void GoToNextPoint()
    {

        if (_patrolPoints.Length == 0) return;

        SetNewDestination(_patrolPoints[destPoint].position);

        destPoint = (destPoint + 1) % _patrolPoints.Length;

        //DebugBalls();
    }

    public void TeleportToPoint(int point)
    {
        if (_agent.Warp(_patrolPoints[point].position))
        {
            Debug.Log("Player moved to another room");
            destPoint = point;
        }
    }

    public void TeleportToRandomPoint()
    {
        TeleportToPoint(Random.Range(0, _patrolPoints.Length - 1));
    }

    // Update is called once per frame
    void Update()
    {
        /*        if (!debugFirstTime) debugFirstTime = true;
                else DebugClearBalls();*/

        if (Active && !_agent.pathPending && _agent.remainingDistance < 0.3f)
        {
            GoToNextPoint();
            if (_agent.pathPending)
            {
                Debug.Log("Next pos " + _agent.nextPosition);
                Debug.Log("Corners " + _agent.path.GetCornersNonAlloc(_agent.path.corners));
                foreach (Vector3 corn in _agent.path.corners)
                {
                    Debug.Log("Corner" + corn);
                }
            }


        }

        else if (hunt)
        {
            SetNewDestination(_player.transform.position);

        }

        if (_agent.pathPending)
        {
            Debug.Log(_agent.nextPosition);
        }

    }

    void SetNewDestination(Vector3 destination)
    {
        _agent.SetDestination(destination);
        if (destination.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void SetHunt()
    {
        active = false;
        hunt = true;
        StartCoroutine(AttackCoroutine());
    }

    public void SetPatrol()
    {
        hunt = false;
        Active = true;
    }

    void GetDirection()
    {
        //_agent.path.corners;
    }

    IEnumerator AttackCoroutine()
    {
        Debug.Log("Waiting for attack");
        while (hunt)
        {
            //Debug.Log("Distance from player " + Mathf.Abs(_spot.position.x - _player.transform.position.x) + " " + Mathf.Abs(_spot.position.y - _player.transform.position.y));
            if (Mathf.Abs(_spot.position.x - _player.transform.position.x) < 0.75f &&
                Mathf.Abs(_spot.position.y - _player.transform.position.y) < 0.75f)
            {
                Debug.Log("Attack!");
                yield return new WaitForSecondsRealtime(_attackReload);
            }
            yield return null;
        }
        yield return null;
    }
}
