using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Duck/WeaponData"), Serializable]
public class WeaponData : ScriptableObject
{
    [SerializeField]
    public string Name;

    [SerializeField]
    public int Damage;

    [SerializeField]
    public int AttackSpeed;

    [SerializeField]
    public int Magazine;

    [SerializeField]
    public float Recoil;

    [SerializeField]
    public int bulletsPerShot;


}
