using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerBasics))]
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] public _Weapon CurrentWeapon;

    [SerializeField] private Animator _animator;

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

    private void Start()
    {
        _animator = transform.GetComponentInChildren<Animator>();
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

            //transform.GetComponentInChildren<Animator>().SetBool(AnimVariable.MeleeAttack, true);
            transform.GetComponentInChildren<Animator>().Play("Base Layer.Protag_1_Knife_Attack_1");
            CurrentWeapon.GetComponent<Collider2D>().enabled = true;
            while (transform.GetComponentInChildren<Animator>().
                GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Protag_1_Knife_Attack_1"))
            {
                //CurrentWeapon.GetComponent<Collider2D>().enabled = true;
            }
            CurrentWeapon.GetComponent<Collider2D>().enabled = false;
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

    internal void AttachKnife(Transform arm, Transform hand)
    {
        CurrentWeapon.transform.position = hand.position;
        CurrentWeapon.transform.rotation = arm.rotation;

    }

}
