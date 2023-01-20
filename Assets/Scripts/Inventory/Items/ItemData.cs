using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Duck/ItemData"), Serializable]
public class ItemData : ScriptableObject
{
    [SerializeField] public string Description;
    [SerializeField] public string ItemName;
    [SerializeField] public int Width = 1;
    [SerializeField] public int Height = 1;

    [SerializeField] public ItemType itemType;

    [SerializeField] public bool IsWeapon;
    [SerializeField] public WeaponData weaponData;
    [SerializeField] public bool IsGun;

    [SerializeField] public bool IsAmmo;
    [SerializeField] public ItemData AmmoWeapon;
    
    [SerializeField] public Sprite ItemIcon;
    [SerializeField] public bool[,] Fill;

    [SerializeField] public ItemData[] Keys;
    [SerializeField] public ItemData[] Results;

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
    [SerializeField, HideInInspector] private Dictionary<ItemData, ItemData> combinable;
    
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

        combinable = new Dictionary<ItemData, ItemData>();
        for(int i = 0; i < Keys.Length; i++)
        {
            combinable.Add(Keys[i], Results[i]);
        }

    }
    public void OnAfterDeserialize()
    {
        // Convert the serializable list into our unserializable array
        Fill = new bool[Width, Height];
        foreach(var package in serializable)
        {
            Fill[package.IndexX, package.IndexY] = package.Element;
        }

        Keys = new ItemData[combinable.Count];
        Results = new ItemData[combinable.Count];
        int index = 0;
        foreach(KeyValuePair<ItemData, ItemData> dict in combinable)
        {
            Keys[index] = dict.Key;
            Results[index] = dict.Value;
            index++;
        }
    }
}
