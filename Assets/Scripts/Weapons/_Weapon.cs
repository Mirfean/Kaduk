using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Weapon : MonoBehaviour
{
    [SerializeField]
    public WeaponType weaponType;
    public string description;
    public int damage;
    public float attackSpeed;
    public WeaponData weaponData;

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

    public void RotateWeapon(bool rotated)
    {
        if (rotated)
        {
            Debug.Log("rotation on");
            transform.Rotate(180f - transform.rotation.x, 0f, 0f);
        }
        else
        {
            Debug.Log("rotation off");
            transform.Rotate(-transform.rotation.x, 0f, 0f);
        }
    }
}
