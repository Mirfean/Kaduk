using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int _hpCurrent;
    [SerializeField] internal int HpMax;

    [SerializeField] int _sanityCurrent;
    [SerializeField] internal int SanityMax;

    [SerializeField] public List<Status> Statuses;

    public int Sanity
    {
        get => _sanityCurrent;
        set => _sanityCurrent = Mathf.Clamp(value, 0, SanityMax);
    }
    public int Hp
    {
        get => _hpCurrent; set
        {
            _hpCurrent = Mathf.Clamp(value, 0, HpMax);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
