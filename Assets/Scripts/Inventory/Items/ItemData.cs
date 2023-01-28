using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Duck/ItemData"), Serializable]
public class ItemData : ScriptableObject
{
    [SerializeField] public string id;
    [SerializeField] public string ItemName;
    [SerializeField] public string Description;
    [SerializeField] public bool IsKeyItem;

    [SerializeField] public int Width = 1;
    [SerializeField] public int Height = 1;
    [SerializeField] public bool[,] Fill;

    [SerializeField] public ItemType itemType;
    [SerializeField] public Sprite ItemIcon;

    [SerializeField] public string[] SecondItem;
    [SerializeField] public string[] Result;

    //Consumable Items
    [SerializeField] public ConsumableEffect Effect;
    [SerializeField] public int EffectPower;

    //Weapon
    [SerializeField] public WeaponType weaponType;
    [SerializeField] public WeaponData weaponData;

    //Ammo
    [SerializeField] public int StackMax;
    [SerializeField] public WeaponType AmmoType;

    public void CreateFill()
    {
        Fill = new bool[Width, Height];
    }

    public void OnEnable()
    {
        if (serializable.Count > 0)
        {
            OnAfterDeserialize();
        }
        else
        {
            CreateFill();
        }
    }

    // A list that can be serialized
    [SerializeField, HideInInspector] private List<SpaceSlots<bool>> serializable;
    // A package to store our stuff
    [System.Serializable]
    struct SpaceSlots<TElement>
    {
        public int IndexX;
        public int IndexY;
        public TElement Element;
        public SpaceSlots(int idx0, int idx1, TElement element)
        {
            IndexX = idx0;
            IndexY = idx1;
            Element = element;
        }
    }


    public void OnBeforeSerialize()
    {
        // Convert our unserializable array into a serializable list
        serializable = new List<SpaceSlots<bool>>();
        for (int i = 0; i < Fill.GetLength(0); i++)
        {
            for (int j = 0; j < Fill.GetLength(1); j++)
            {
                serializable.Add(new SpaceSlots<bool>(i, j, Fill[i, j]));
                Debug.Log("ELO");
            }
        }

    }
    public void OnAfterDeserialize()
    {
        // Convert the serializable list into our unserializable array
        Fill = new bool[Width, Height];
        foreach (var package in serializable)
        {
            Fill[package.IndexX, package.IndexY] = package.Element;
        }
    }
}
