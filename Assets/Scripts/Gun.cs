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

    Vector2 movement;
    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Rigidbody2D rb = new Rigidbody2D();

        
    }

    public override void Attack(Vector2 mousePos)
    {
        Debug.Log("Shooting by " + name);
        Vector2 normXY = normalizeMousePos(mousePos);

        RotateAim(mousePos);
        GameObject newBullet = Instantiate(bullet, shootingPoint.transform.position, shootingPoint.transform.rotation);

        newBullet.GetComponent<Rigidbody2D>().AddForce(shootingPoint.transform.forward * bulletSpeed, ForceMode2D.Impulse);
        Destroy(newBullet, 10f);
    }

    public Vector2 normalizeMousePos(Vector2 mousePos)
    {
        Vector2 result = Mathf.Abs(mousePos.x) >= Mathf.Abs(mousePos.y) ? new Vector2(mousePos.x / Mathf.Abs(mousePos.x), mousePos.y/ Mathf.Abs(mousePos.x)) : new Vector2(mousePos.x / Mathf.Abs(mousePos.y), mousePos.y / Mathf.Abs(mousePos.y));
        Debug.Log("Normalized vector " + result.x + " " + result.y);
        return result;
    }

    public void RotateAim(Vector2 mousePos)
    {
        Vector2 aimDir = mousePos - new Vector2(shootingPoint.transform.position.x, shootingPoint.transform.position.y);
        float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;
        shootingPoint.GetComponent<Rigidbody2D>().rotation = aimAngle;
    }
}
