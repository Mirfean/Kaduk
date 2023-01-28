using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class HealthSanityStatus : MonoBehaviour
{
    public HpSanityEnum P_Hp;

    public HpSanityEnum P_Sanity;

    PlayerStats stats;

    [SerializeField]
    Image HpStatus;

    [SerializeField]
    Image SanityStatus;

    [SerializeField] Sprite[] HealthImages;

    [SerializeField] Sprite[] SanityImages;

    private void Start()
    {
        stats = FindObjectOfType<PlayerStats>();

        UpdateHpStatus();
        UpdateSanityStatus();

        //ChangeStats();
    }

    private void Update()
    {
        UpdateHpStatus();
        UpdateSanityStatus();
    }

    public void UpdateHpStatus()
    {
        if (stats.Hp > stats.HpMax / 5)
        {
            if (stats.Hp > (stats.HpMax / 5) * 2)
            {
                if (stats.Hp > (stats.HpMax / 5) * 3)
                {
                    if (stats.Hp > (stats.HpMax / 5) * 4)
                    {
                        P_Hp = HpSanityEnum.P80;
                    }
                }
                else P_Hp = HpSanityEnum.P60;
            }
            else P_Hp = HpSanityEnum.P40;
        }
        else P_Hp = HpSanityEnum.P20;

        ChangeStats();
    }

    public void UpdateSanityStatus()
    {
        if (stats.Sanity > stats.SanityMax / 5)
        {
            if (stats.Sanity > (stats.SanityMax / 5) * 2)
            {
                if (stats.Sanity > (stats.SanityMax / 5) * 3)
                {
                    if (stats.Sanity > (stats.SanityMax / 5) * 4)
                    {
                        P_Sanity = HpSanityEnum.P80;
                    }
                }
                else P_Sanity = HpSanityEnum.P60;
            }
            else P_Sanity = HpSanityEnum.P40;
        }
        else P_Sanity = HpSanityEnum.P20;

        ChangeStats();
    }

    //Change Health and Sanity Images depends of Hp/Sanity status
    public void ChangeStats()
    {
        switch (P_Hp)
        {
            case HpSanityEnum.P100:
                break;
            case HpSanityEnum.P80:
                HpStatus.sprite = HealthImages[0];
                break;
            case HpSanityEnum.P60:
                HpStatus.sprite = HealthImages[1];
                break;
            case HpSanityEnum.P40:
                HpStatus.sprite = HealthImages[2];
                break;
            case HpSanityEnum.P20:
                HpStatus.sprite = HealthImages[3];
                break;
        }
        switch (P_Sanity)
        {
            case HpSanityEnum.P100:
                break;
            case HpSanityEnum.P80:
                SanityStatus.sprite = SanityImages[0];
                break;
            case HpSanityEnum.P60:
                SanityStatus.sprite = SanityImages[1];
                break;
            case HpSanityEnum.P40:
                SanityStatus.sprite = SanityImages[2];
                break;
            case HpSanityEnum.P20:
                SanityStatus.sprite = SanityImages[3];
                break;
            case HpSanityEnum.P0:
                break;
        }

    }


}
