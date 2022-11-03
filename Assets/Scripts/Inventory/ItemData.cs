using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Duck/ItemData"), Serializable]
public class ItemData : ScriptableObject
{
    [SerializeField]
    public string description;

    [SerializeField]
    public int width = 1;
    [SerializeField]
    public int height = 1;
    [SerializeField]
    public Sprite itemIcon;

    [SerializeField]
    public bool[,] fill;


    public void CreateFill()
    {
        fill = new bool[width, height];
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
    [SerializeField, HideInInspector] private List<Package<bool>> serializable;
    // A package to store our stuff
    [System.Serializable]
    struct Package<TElement>
    {
        public int Index0;
        public int Index1;
        public TElement Element;
        public Package(int idx0, int idx1, TElement element)
        {
            Index0 = idx0;
            Index1 = idx1;
            Element = element;
        }
    }
    public void OnBeforeSerialize()
    {
        // Convert our unserializable array into a serializable list
        serializable = new List<Package<bool>>();
        for (int i = 0; i < fill.GetLength(0); i++)
        {
            for (int j = 0; j < fill.GetLength(1); j++)
            {
                serializable.Add(new Package<bool>(i, j, fill[i, j]));
                Debug.Log("ELO");
            }
        }
    }
    public void OnAfterDeserialize()
    {
        // Convert the serializable list into our unserializable array
        fill = new bool[width, height];
        foreach(var package in serializable)
        {
            fill[package.Index0, package.Index1] = package.Element;
        }
    }
}
