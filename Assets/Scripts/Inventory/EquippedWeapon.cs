using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedWeapon : MonoBehaviour
{
    [SerializeField] Image _weaponImage;  

    [SerializeField] public _Weapon CurrentWeapon;

    void ChangeWeapon(WeaponType weaponToChange)
    {
        
    }

    public void ChangeWeapon(_Weapon newWeapon)
    {
        CurrentWeapon = newWeapon;
        _weaponImage.sprite = CurrentWeapon.weaponData.WeaponIcon;
    }
}
