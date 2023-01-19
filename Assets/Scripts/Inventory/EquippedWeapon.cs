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
        CurrentWeapon.transform.rotation = new Quaternion();
        _weaponImage.sprite = CurrentWeapon.weaponData.WeaponIcon;
        if (CurrentWeapon.weaponType == WeaponType.KNIFE)
        {
            _weaponImage.transform.Rotate(0, 0, 270f);
        }
        _weaponImage.GetComponent<RectTransform>().sizeDelta =
            new Vector2(_weaponImage.preferredWidth, _weaponImage.preferredHeight);
    }
}
