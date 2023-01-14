using Assets.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerControl))]
public class PlayerWeapon : MonoBehaviour
{
    //[SerializeField] public _Weapon CurrentWeapon;

    [SerializeField] public GameObject CurrentWeapon;

    [SerializeField] List<GameObject> WeaponPrefabs;

    [SerializeField] Transform _attachments;

    [SerializeField] private Animator _animator;

    [SerializeField]
    private bool _rotated = false;
    public bool Rotated
    {
        get { return _rotated; }
        set
        {
            if (CurrentWeapon != null)
            {
                if (CurrentWeapon.activeSelf && _rotated != value)
                {
                    RotateWeapon(value);
                }
                
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
        if (CurrentWeapon != null)
        {
            if (CurrentWeapon.GetComponent<_Weapon>() is Gun)
            {
                return true;
            }
        }
        return false;
    }

    public void ShowWeapon()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.SetActive(true);
        }
    }

    public void HideWeapon()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.SetActive(false);
        }
    }

    public void UseWeapon(SkeletalMove skeletalMove, Player playerInput) {
        if (CurrentWeapon != null)
        {
            if (CurrentWeapon.GetComponent<_Weapon>() is Gun)
            {
                if (CurrentWeapon.GetComponent<Gun>().remainingBullets > 0)
                    CurrentWeapon.GetComponent<_Weapon>().Attack(
                        Camera.main.ScreenToWorldPoint(playerInput.Basic.MouseMovement.ReadValue<Vector2>()),
                        skeletalMove.HoldedItem.rotation);
                else
                {
                    Debug.Log("Reload required!");
                }
            }
            else if (CurrentWeapon.GetComponent<_Weapon>() is MeleeWeapon)
            {
                //string attackAnimation = "Base Layer.Protag_1_Knife_Attack_1";
                string attackAnimation = "Base Layer.Protag_1_Knife_Attack_2_stab";
                if (!IsAnimationRunning(attackAnimation))
                {
                    transform.GetComponentInChildren<Animator>().Play(attackAnimation);
                    StartCoroutine(KnifeCoroutine(attackAnimation));
                }

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

    void RotateWeapon(bool rotated)
    {
        if(CurrentWeapon != null)
        {
            CurrentWeapon.GetComponent<_Weapon>().RotateWeapon(rotated);
        }
        
    }

    internal void AttachKnife(Transform arm, Transform hand)
    {
        CurrentWeapon.transform.position = hand.position;
        Quaternion x = arm.rotation;
        x.z += hand.localRotation.z;
        CurrentWeapon.transform.rotation = x;
    }

    internal Animator GetAnimator()
    {
        return transform.GetComponentInChildren<Animator>();
    }

    internal bool IsAnimationRunning(string animName)
    {
        return GetAnimator().GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    internal void ChangeWeapon(GameObject newWeapon)
    {
        if(CurrentWeapon != null) Destroy(CurrentWeapon.gameObject);
        CurrentWeapon = newWeapon;
        Instantiate(CurrentWeapon).transform.SetParent(_attachments);
        /*        foreach(GameObject weapon in WeaponPrefabs)
                {
                    if (weapon.GetComponent<_Weapon>() == newWeapon)
                    {
                        CurrentWeapon = weapon;
                        Instantiate(weapon).transform.SetParent(_attachments);

                    }
                }*/
    }

}
