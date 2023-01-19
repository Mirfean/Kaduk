using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItemData : MonoBehaviour
{
    [SerializeField] public bool IsHeal;
    [SerializeField] public int HealPower;

    [SerializeField] public bool IsSanity;
    [SerializeField] public int SanityPower;
}
