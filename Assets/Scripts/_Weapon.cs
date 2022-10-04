using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Weapon : MonoBehaviour
{
    public string description;
    public float damage;
    public float attackSpeed;

    public virtual void Attack(Vector2 mousePos)
    {
        Debug.Log("Attacking by " +name);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
