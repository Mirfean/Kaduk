using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHp;

    [SerializeField] public bool _dead = false;

    [SerializeField] private bool immune = false;

    [SerializeField] private int _currentHp;

    [SerializeField] float _attackDistance;

    public SpriteRenderer EnemySprite;
    public int CurrentHp
    {
        get { return _currentHp; }
        set
        {
            _currentHp = value;
        }
    }

    [SerializeField] public EnemyInfo EnemyData;

    // Start is called before the first frame update
    void Start()
    {
        if(EnemySprite == null) EnemySprite = GetComponentInChildren<SpriteRenderer>();
        _maxHp = EnemyData._maxHp;
        CurrentHp = _maxHp;
    }

    private bool IsInAttackRange(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance < _attackDistance;
    }

    public void TakeDamage(int damage, WeaponType weaponType)
    {
        if (!immune)
        {
            GetComponent<Animator>().Play("GetHit");

            CurrentHp -= (damage - EnemyData._defence);
            StartCoroutine(HitFlash());

            if (weaponType is WeaponType.KNIFE or WeaponType.AXE or WeaponType.PIPE)
            {
                StartCoroutine(ImmuneCoroutine());
            }
            if (CurrentHp <= 0)
            {
                _dead = true;
                DeathSequence();
            }
        }
    }

    IEnumerator ImmuneCoroutine()
    {
        immune = true;
        yield return new WaitForSecondsRealtime(0.5f);
        immune = false;
    }

    IEnumerator HitFlash()
    {
        EnemySprite.color = new Color(0.9f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        EnemySprite.color = new Color(255f, 255f, 255f);
    }

    private void DeathSequence()
    {
        foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
        //GetComponent<AudioSource>().loop = false;
        GetComponent<EnemyMovement>().Death();
        
        //Destroy(gameObject);
        
    }



}
