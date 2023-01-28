using Assets.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

public class Gun : _Weapon
{

    [SerializeField]
    GameObject _shootingPoint;

    [SerializeField]
    Transform _holdedItem;

    [SerializeField, Range(0f, 1f)]
    float _recoil;

    [SerializeField]
    GameObject _bullet;

    [SerializeField]
    [Range(0.1f, 10f)]
    float _bulletSpeed;

    [SerializeField]
    [Range(0.05f, 2)]
    float _fireRate = 0.5f;

    [SerializeField]
    float _nextFire;

    private AudioSource gunAudio;

    private LineRenderer laserLine;

    [SerializeField]
    private GameObject _bulletTrail;

    [SerializeField]
    private Animator _muzzleFlashAnimator;

    [SerializeField]
    private float _bulletRange;

    [SerializeField]
    internal int maxBullets;

    [SerializeField]
    internal int remainingBullets;

    private void Start()
    {
        if (Damage == 0) Damage = WeaponInfo.Damage;
    }

    //TODO SHOTGUN AND RIFLE
    public override void Attack(Vector2 mousePos, Quaternion holdItemRot)
    {
        Debug.Log("Shooting by " + name);

        //_muzzleFlashAnimator.SetTrigger("Shoot");

        Vector3 direction;
        RaycastHit2D hit = new RaycastHit2D();

        switch (weaponType)
        {
            case WeaponType.HANDGUN:
                direction = MakeSpreadDirection();
                //Debug.Log("Direction " + direction);
                RotateAim(mousePos, holdItemRot);
                hit = Physics2D.Raycast(
                    _shootingPoint.transform.position,
                    direction,
                    _bulletRange,
                    ~layerEnemyToIgnore);
                if (hit.collider == null) Debug.DrawLine(_shootingPoint.transform.position, direction * _bulletRange, Color.red, duration: 60f);
                if (hit.collider != null)
                {
                    /*                    Debug.Log("Hitted " + hit.collider.name);
                                        Debug.DrawLine(_shootingPoint.transform.position, hit.collider.transform.position, Color.cyan, duration: 60f);

                                        if (hit.collider.gameObject.layer == 13) Debug.Log("Hit Enemy Core!");
                                        if (hit.collider.gameObject.layer == 23) Debug.Log("Hit Enemy Limb!");*/
                    if (hit.collider.GetComponent<HitTarget>())
                    {
                        Debug.Log("Bullet hitted enemy!");
                        hit.collider.GetComponent<HitTarget>().TakeHit(Damage, weaponType);
                    }
                    Debug.Log("Bullet");
                }
                else
                {
                    var endPosition = _shootingPoint.transform.position + transform.right * _bulletRange;
                    Debug.Log("end " + endPosition);
                }
                break;
            case WeaponType.SHOTGUN:
                List<RaycastHit2D> hits = new List<RaycastHit2D>();
                for (int i = 0; i < 7; i++)
                {
                    direction = MakeSpreadDirection();
                    RotateAim(mousePos, holdItemRot);
                    hit = Physics2D.Raycast(
                        _shootingPoint.transform.position,
                        direction,
                        _bulletRange);
                    hits.Add(hit);
                }
                foreach (var bullet in hits)
                {
                    if (bullet.collider != null)
                    {
                        if (bullet.collider.GetComponent<HitTarget>())
                        {
                            bullet.collider.GetComponent<HitTarget>().TakeHit(Damage, weaponType);
                        }
                    }
                    else
                    {
                        var endPosition = _shootingPoint.transform.position + transform.right * _bulletRange;
                        Debug.Log("end " + endPosition);
                    }
                }
                //TODO 
                break;
            case WeaponType.RIFLE:
                //TODO
                break;

        }
        //var trail = Instantiate(_bulletTrail, _shootingPoint.transform.position, _shootingPoint.transform.rotation);

        Debug.Log("Transform right " + transform.right);

        //var trailScript = trail.GetComponent<BulletTrail>();



    }

    private Vector3 MakeSpreadDirection()
    {
        Vector3 direction = transform.right;
        Vector3 spread = Vector3.zero + (transform.up * Random.Range(-1f, 1f)) + (transform.right * Random.Range(-1f, 1f));
        spread.Normalize();
        direction += spread * Random.Range(0f, _recoil);
        return direction;
    }

    public Vector2 normalizeMousePos(Vector2 mousePos)
    {
        Vector2 result = Mathf.Abs(mousePos.x) >= Mathf.Abs(mousePos.y) ? new Vector2(mousePos.x / Mathf.Abs(mousePos.x), mousePos.y / Mathf.Abs(mousePos.x)) : new Vector2(mousePos.x / Mathf.Abs(mousePos.y), mousePos.y / Mathf.Abs(mousePos.y));
        Debug.Log("Normalized vector " + result.x + " " + result.y);
        return result;
    }

    //To remove?
    public void RotateAim(Vector2 mousePos, Quaternion holdItemRot)
    {
        Vector2 aimDiff = new Vector2(mousePos.x - _shootingPoint.transform.position.x - transform.position.x, mousePos.y - _shootingPoint.transform.position.y - transform.position.y);
        float aimAngle = (Mathf.Atan2(aimDiff.y, aimDiff.x) * Mathf.Rad2Deg);
        //Debug.Log($"aimAngle {aimAngle} and gun rotation {transform.rotation.z} and holdItemRot {holdItemRot.z}");
    }

    public void RotateGun(bool rotated)
    {
        if (rotated)
        {
            Debug.Log("rotation on");
            SetGunRotation(true);
        }
        else
        {
            Debug.Log("rotation off");
            SetGunRotation(false);
        }
    }

    public void SetGunRotation(bool rotated)
    {
        if (rotated)
        {
            transform.Rotate(180f - transform.rotation.x, 0f, 0f);
            //transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
        else
        {
            transform.Rotate(-transform.rotation.x, 0f, 0f);
            //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
