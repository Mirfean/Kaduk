using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : _Weapon
{
    [SerializeField]
    GameObject shootingPoint;

    [SerializeField, Range(0f, 1f)]
    float recoil;

    [SerializeField]
    GameObject bullet;

    [SerializeField][Range(0.1f, 10f)]
    float bulletSpeed;

    [SerializeField][Range(0.05f, 2)]
    float fireRate;

    [SerializeField]
    float nextFire;
    
    Vector2 movement;
    Vector2 mousePos;

    private AudioSource gunAudio;

    private LineRenderer laserLine;



    // Start is called before the first frame update
    void Start()
    {
        fireRate = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    public override void Attack(Vector2 mousePos, Quaternion holdItemRot)
    {
        Debug.Log("Shooting by " + name);
        //Vector2 normXY = normalizeMousePos(mousePos);
        //RotateAim(mousePos);
        //GameObject newBullet = Instantiate(bullet, shootingPoint.transform.position, shootingPoint.transform.rotation);
        if(Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            RotateAim(mousePos, holdItemRot);
            GameObject newBullet = Instantiate(bullet, shootingPoint.transform.position, shootingPoint.transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
            Destroy(newBullet, 10f);
        }
        
    }

    public Vector2 normalizeMousePos(Vector2 mousePos)
    {
        Vector2 result = Mathf.Abs(mousePos.x) >= Mathf.Abs(mousePos.y) ? new Vector2(mousePos.x / Mathf.Abs(mousePos.x), mousePos.y/ Mathf.Abs(mousePos.x)) : new Vector2(mousePos.x / Mathf.Abs(mousePos.y), mousePos.y / Mathf.Abs(mousePos.y));
        Debug.Log("Normalized vector " + result.x + " " + result.y);
        return result;
    }

    public void RotateAim(Vector2 mousePos, Quaternion holdItemRot)
    {
        Debug.Log("gun rotation " + transform.rotation.z);
        
        //Vector2 aimDiff = new Vector2(mousePos.x - shootingPoint.transform.position.x, mousePos.y - shootingPoint.transform.position.y);
        Vector2 aimDiff = new Vector2(mousePos.x - shootingPoint.transform.position.x - transform.position.x, mousePos.y - shootingPoint.transform.position.y - transform.position.y);
        //aimDiff.Normalize();
        float aimAngle = (Mathf.Atan2(aimDiff.y, aimDiff.x) * Mathf.Rad2Deg);
        //shootingPoint.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
        Debug.Log($"aimAngle {aimAngle} and gun rotation {transform.rotation.z} and holdItemRot {holdItemRot.z}");
    }

    public void RotateGun(bool rotated)
    {
        if (rotated)
        {
            //transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            Debug.Log("Go on");

            float diff = 180 - transform.rotation.x;

            transform.Rotate(diff, 0f, 0f);

        }
        else
        {
            float diff = 0 - transform.rotation.x;

            transform.Rotate(diff, 0f, 0f);
            //transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            Debug.Log("Go off");
            //gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
