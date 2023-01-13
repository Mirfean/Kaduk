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

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
        _agent.updatePosition = true;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    void GoToNextPoint()
    {
        if (_patrolPoints.Length == 0) return;

        _agent.SetDestination(_patrolPoints[destPoint].position);

        destPoint = (destPoint + 1) % _patrolPoints.Length;

    }

    public void GoToPoint(int point)
    {
        if (_agent.Warp(_patrolPoints[point].position))
        {
            Debug.Log("Player moved to another room");
            destPoint = point;
        }
    }

    public void GoToRandomPoint()
    {
        GoToPoint(Random.Range(0, _patrolPoints.Length - 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (Active && !_agent.pathPending && _agent.remainingDistance < 0.3f)
        {
            GoToNextPoint();
        }

        else if (hunt)
        {
            _agent.SetDestination(_player.transform.position);
        }
    }
}
