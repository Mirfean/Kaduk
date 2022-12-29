using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerBasics))]
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] public _Weapon CurrentWeapon;

    [SerializeField]
    private bool _rotated = false;
    public bool Rotated
    {
        get { return _rotated; }
        set
        {
            if (CurrentWeapon.gameObject.activeSelf && _rotated != value)
            {
                RotateWeapon(value);
            }
            _rotated = value;

        }
    }

    public bool IsItGun()
    {
        if (CurrentWeapon is Gun)
        {
            return true;
        }
        return false;
    }

    public void UseWeapon(SkeletalMove skeletalMove, Player playerInput) {
        if (CurrentWeapon is Gun)
        {
            if(CurrentWeapon.GetComponent<Gun>().remainingBullets > 0)
            CurrentWeapon.Attack(
                Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>()),
                skeletalMove.HoldedItem.rotation);
            else
            {
                Debug.Log("Reload required!");
            }
        }
        else if (CurrentWeapon is MeleeWeapon)
        {
            /* Activate trigger
             * Animation
             * Attack
             * combo?
             */
        }
    }

    internal void ShowWeapon()
    {
        CurrentWeapon.gameObject.SetActive(true);
    }

    internal void HideWeapon()
    {
       CurrentWeapon.gameObject.SetActive(false);
    }

    void RotateWeapon(bool rotated)
    {
        CurrentWeapon.RotateWeapon(rotated);
    }

    

}
