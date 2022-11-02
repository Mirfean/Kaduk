using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Duck/ItemData")]
public class ItemData : ScriptableObject
{
    public string description;

    public Matrix4x4 fill;

    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;

}
