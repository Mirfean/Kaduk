using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeapon : MonoBehaviour
{
    [SerializeField]
    Image _weapon;

    [SerializeField]
    List<Image> weapons;

    void ChangeWeapon(WeaponType weaponToChange)
    {
        
    }
}
