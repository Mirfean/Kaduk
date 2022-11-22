using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Info", menuName = "Duck/EnemyInfo"), Serializable]
public class EnemyInfo : ScriptableObject
{
    [SerializeField] public string _name;

    [SerializeField] public int _maxHp;


    [SerializeField] public int _defence;

    [SerializeField] public int _dmg;
    [SerializeField] public float _speed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
