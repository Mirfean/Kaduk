using Assets.Scripts.Enums;
using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HitTarget : MonoBehaviour
{
    [SerializeField] Collider2D _collider;
    [SerializeField] Enemy _parentEnemy;
    [SerializeField] LimbHit _limbHit;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _parentEnemy = GetComponentInParent<Enemy>();
        if (Enum.TryParse(gameObject.tag, out _limbHit))
        {
            _limbHit = Enum.Parse<LimbHit>(gameObject.tag);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeHit(int damage, WeaponType weaponType)
    {
        switch (_limbHit)
        {
            case LimbHit.NormalLimb:
                {
                    _parentEnemy.TakeDamage(damage, weaponType);
                    break;
                }
            case LimbHit.CritLimb:
                {
                    _parentEnemy.TakeDamage(damage + (damage / 2), weaponType);
                    break;
                }
            case LimbHit.LessLimb:
                {
                    _parentEnemy.TakeDamage(damage / 2, weaponType);
                    break;
                }

        }
    }
}
