using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHp;

    [SerializeField] private int _currentHp;

    [SerializeField] float _attackDistance;

    public SpriteRenderer SpriteRenderer;
    public int CurrentHp
    {
        get { return _currentHp; }
        set 
        {
            _currentHp = value; 
        }
    }

    [SerializeField] public EnemyInfo EnemyData;

    // Start is called before the first frame update
    void Start()
    {
        this.SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _maxHp = EnemyData._maxHp;
        CurrentHp = _maxHp;
    }

    private bool IsInAttackRange(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < _attackDistance;
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= (damage - EnemyData._defence);
        if (CurrentHp <= 0) DeathSequence();
    }

    IEnumerator ImmuneCoroutine()
    {

    }

    private void DeathSequence()
    {
        //TODO
        Destroy(gameObject);
    }
}
