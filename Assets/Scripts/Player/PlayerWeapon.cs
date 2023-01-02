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
            string attackAnimation = "Base Layer.Protag_1_Knife_Attack_1";
            if (!IsAnimationRunning(attackAnimation)){
                transform.GetComponentInChildren<Animator>().Play(attackAnimation);
                StartCoroutine(KnifeCoroutine(attackAnimation));
            }
            
        }
    }

    internal IEnumerator KnifeCoroutine(string animationName)
    {
        CurrentWeapon.GetComponent<Collider2D>().enabled = true;
        do
        {
            yield return new WaitForEndOfFrame();
        } while (IsAnimationRunning(animationName));
        CurrentWeapon.GetComponent<Collider2D>().enabled = false;
        yield return null;
    }

    internal IEnumerator AttackCoroutine()
    {
        yield break;
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

    internal Animator GetAnimator()
    {
        return transform.GetComponentInChildren<Animator>();
    }

    internal bool IsAnimationRunning(string animName)
    {
        return GetAnimator().GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

}
