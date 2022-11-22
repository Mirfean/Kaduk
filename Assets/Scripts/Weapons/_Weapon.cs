using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Weapon : MonoBehaviour
{
    public string Description;
    public int Damage;
    public float AttackSpeed;
    //public WeaponType

    public bool Rotated = false;

    public virtual void Attack(Vector2 mousePos)
    {
        Debug.Log("Attacking by " +name);
    }

    public virtual void Attack(Vector2 mousePos, Quaternion quaternion)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual float GetRealRotationToMouse()
    {
        Debug.Log("Base Weapon rotation");
        return transform.rotation.z;
    }
}
