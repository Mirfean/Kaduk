using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] int destPoint = 0;

    bool _active;
    bool _patrol;
    bool _hunt;
    bool _dead;

    public bool Active
    {
        get { return _active; }
        set
        {
            Animator.SetBool("active", value);
            _active = value;
        }
    }
    
    public bool Patrol
    {
        get { return _patrol; }
        set
        {
            Animator.SetBool("patrol", value);
            _patrol = value;
        }
    }
    
    public bool Hunt
    {
        get { return _hunt; }
        set
        {
            Animator.SetBool("hunt", value);
            _hunt = value;
        }
    }

    [SerializeField] bool _waitingForAttack;

    private NavMeshAgent _agent;

    [SerializeField] private NavMeshAgent _player;

    public bool debugFirstTime;
    public List<GameObject> PathBalls;
    public GameObject Ballprefab;

    [SerializeField] Transform _spot;


    [SerializeField] float _attackReload;

    public Animator Animator;

    [SerializeField] Transform AttackCenter;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;
        _agent.updatePosition = true;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        //For testing
        Active = true;
        SetPatrol();

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

    internal void Death()
    {
        Animator.SetBool("dead", true);
        _agent.ResetPath();
        Active = false;
        Hunt = false;
        Patrol = false;
        _dead = true;
        Animator.SetBool("attack", false);
    }

    public void TeleportToRandomPoint()
    {
        TeleportToPoint(Random.Range(0, _patrolPoints.Length - 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (!_dead)
        {
            if (Active && Patrol && !_agent.pathPending && _agent.remainingDistance < 0.25f)
            {
                GoToNextPoint();
                /*if (_agent.pathPending)
                {
                    Debug.Log("Next pos " + _agent.nextPosition);
                    Debug.Log("Corners " + _agent.path.GetCornersNonAlloc(_agent.path.corners));
                    foreach (Vector3 corn in _agent.path.corners)
                    {
                        Debug.Log("Corner" + corn);
                    }
                }*/


            }

            else if (Hunt)
            {
                SetNewDestination(_player.transform.position);
                if (!_waitingForAttack) StartCoroutine(AttackCoroutine());
            }
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
        Patrol = false;
        Hunt = true;

    }

    public void SetPatrol()
    {
        Hunt = false;
        Patrol = true;
    }

    IEnumerator AttackCoroutine()
    {
        Debug.Log("Waiting for attack");
        _waitingForAttack = true;
        while (Hunt)
        {
            //Debug.Log("Distance " + _agent.remainingDistance);
            if (_agent.remainingDistance < 1 && !Animator.GetBool("attack"))
            {
                Debug.Log("Attack!");
                Animator.SetBool("attack", true);
                _agent.ResetPath();
                
                StartCoroutine(Attack());
            }
            yield return null;
        }
        Debug.Log("Stop waiting for attack");
        _waitingForAttack=false;
    }

    IEnumerator Attack()
    {
        while (Animator.GetBool("attack"))
        {
            Debug.Log("Attack in progress");
            Collider2D hitPlayer = Physics2D.OverlapCapsule(
                    AttackCenter.position + new Vector3(0, -0.35f), new Vector2(0.7f, 1.9f), CapsuleDirection2D.Vertical, 0f, LayerMask.GetMask("Player"));
            if (hitPlayer != null)
            {
                Debug.Log("Player Hitted!" + hitPlayer.name);
                hitPlayer.gameObject.GetComponent<PlayerStats>().Hp = - GetComponent<Enemy>().EnemyData._dmg;
                yield break;
            }
            yield return null;
        }
    }
}
