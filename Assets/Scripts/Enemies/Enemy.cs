using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHp;

    [SerializeField] private int _currentHp;

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
        _maxHp = EnemyData._maxHp;
        CurrentHp = _maxHp;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Dealing " + (damage - EnemyData._defence));
        CurrentHp -= (damage - EnemyData._defence);
        if (CurrentHp <= 0) DeathSequence();
    } 

    private void DeathSequence()
    {
        //TODO
        Destroy(gameObject);
    }
}
