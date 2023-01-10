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

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
        _agent.updatePosition = true;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    void GotoNextPoint()
    {
        if (_patrolPoints.Length == 0) return;

        _agent.SetDestination(_patrolPoints[destPoint].position);

        destPoint = (destPoint + 1) % _patrolPoints.Length;

    }



    // Update is called once per frame
    void Update()
    {
        if (active && !_agent.pathPending && _agent.remainingDistance < 0.3f)
        {
            GotoNextPoint();
        }

        else if (hunt)
        {
            _agent.SetDestination(_player.transform.position);
        }
    }


}
