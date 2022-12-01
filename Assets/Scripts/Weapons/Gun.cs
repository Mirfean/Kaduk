using System.Collections;
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

    [SerializeField][Range(0.1f, 10f)]
    float _bulletSpeed;

    [SerializeField][Range(0.05f, 2)]
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

    public override void Attack(Vector2 mousePos, Quaternion holdItemRot)
    {
        Debug.Log("Shooting by " + name);

        //_muzzleFlashAnimator.SetTrigger("Shoot");

        Vector3 direction = MakeSpreadDirection();

        Debug.DrawRay(_shootingPoint.transform.position, direction, Color.blue, _bulletRange, true);

        RotateAim(mousePos, holdItemRot);
        var hit = Physics2D.Raycast(
            _shootingPoint.transform.position,
            direction,
            _bulletRange);

        var trail = Instantiate(_bulletTrail, _shootingPoint.transform.position, _shootingPoint.transform.rotation);

        Debug.Log("Transform right " + transform.right);


        //Debug.DrawRay(_shootingPoint.transform.position, transform.right - new Vector3(0.25f, 0f, 0f), Color.black, _bulletRange, true);
        //Debug.DrawRay(_shootingPoint.transform.position, transform.right - new Vector3(0.5f, 0f, 0f), Color.red, _bulletRange, true);
        //Debug.DrawRay(_shootingPoint.transform.position, transform.right , Color.white, _bulletRange, true);
        //Debug.DrawRay(_shootingPoint.transform.position, transform.right + new Vector3(0.25f, 0f, 0f), Color.green, _bulletRange, true);
        //Debug.DrawRay(_shootingPoint.transform.position, transform.right + new Vector3(0.5f, 0f, 0f), Color.blue, _bulletRange, true);

        var trailScript = trail.GetComponent<BulletTrail>();

        if (hit.collider != null)
        {
            //Damage enemies etc.
            //trailScript.SetTargetPosition(hit.point);
/*            if (hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(Damage);
                Debug.Log("I hit enemy!");
            }*/

            if(hit.collider.GetComponent<HitTarget>())
            {
                hit.collider.GetComponent<HitTarget>().TakeHit(Damage);
            
            }
        }
        else
        {
            var endPosition = _shootingPoint.transform.position + transform.right * _bulletRange;
            //trailScript.SetTargetPosition(endPosition);
            Debug.Log("end " + endPosition);
        }

    }

    private Vector3 MakeSpreadDirection()
    {
        Vector3 direction = transform.right;
        Vector3 spread = Vector3.zero + (transform.up * Random.Range(-1f, 1f)) + (transform.right * Random.Range(-1f, 1f));
        spread.Normalize();
        direction += spread * Random.Range(0f, 0.4f);
        return direction;
    }

    //old
    private void ShootBullet()
    {
        //Shooting projectile
        /*if(Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            RotateAim(mousePos, holdItemRot);
            GameObject newBullet = Instantiate(_bullet, _shootingPoint.transform.position, _shootingPoint.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * _bulletSpeed, ForceMode2D.Impulse);
            Destroy(newBullet, 10f);
        }*/

    }

    public Vector2 normalizeMousePos(Vector2 mousePos)
    {
        Vector2 result = Mathf.Abs(mousePos.x) >= Mathf.Abs(mousePos.y) ? new Vector2(mousePos.x / Mathf.Abs(mousePos.x), mousePos.y/ Mathf.Abs(mousePos.x)) : new Vector2(mousePos.x / Mathf.Abs(mousePos.y), mousePos.y / Mathf.Abs(mousePos.y));
        Debug.Log("Normalized vector " + result.x + " " + result.y);
        return result;
    }

    //To remove?
    public void RotateAim(Vector2 mousePos, Quaternion holdItemRot)
    {
        Vector2 aimDiff = new Vector2(mousePos.x - _shootingPoint.transform.position.x - transform.position.x, mousePos.y - _shootingPoint.transform.position.y - transform.position.y);
        float aimAngle = (Mathf.Atan2(aimDiff.y, aimDiff.x) * Mathf.Rad2Deg);
        Debug.Log($"aimAngle {aimAngle} and gun rotation {transform.rotation.z} and holdItemRot {holdItemRot.z}");
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
