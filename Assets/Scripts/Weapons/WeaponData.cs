using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Duck/WeaponData"), Serializable]
public class WeaponData : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public int Damage;
    [SerializeField] public float AttackSpeed;

    [SerializeField] public bool IsGun;
    [SerializeField] public int BulletsPerShot;
    [SerializeField] public int Magazine;
    [SerializeField] public float Recoil;

    [SerializeField] public Sprite WeaponIcon;
    [SerializeField] public GameObject WeaponPrefab;
}
