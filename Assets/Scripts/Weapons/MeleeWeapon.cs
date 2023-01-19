using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : _Weapon
{

    Collider2D m_Collider;

    // Start is called before the first frame update
    void Start()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HitTarget>())
        {
            collision.gameObject.GetComponent<HitTarget>().TakeHit(damage, weaponType);
        }
    }
}
