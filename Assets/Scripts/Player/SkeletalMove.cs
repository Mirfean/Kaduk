using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SkeletalMove : MonoBehaviour
{
    [SerializeField]
    private SpriteSkin _skeleton;

    [SerializeField]
    public Transform LeftHand;

    [SerializeField]
    public Transform RightHand;

    [SerializeField]
    public Transform RightArm;

    [SerializeField]
    private PlayerControl _player;

    [SerializeField]
    Animator _animator;

    [SerializeField]
    bool _isAiming;

    [SerializeField]
    public Transform HoldedItem;

/*    [SerializeField]
    float _holdItemRotationIdle = -68f;*/

    [SerializeField]
    GameObject _flashlight;

    public GameObject Flashlight { get { return _flashlight; } private set { _flashlight = value; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Setting HoldItem to Idle basic status(Z rotation to -68)
    /// </summary>
/*    public void SetArmsToIdle()
    {
        HoldedItem.rotation = Quaternion.Euler(0, 0, _holdItemRotationIdle);
    }*/

    public void TrackCursorByHands(Vector2 mousePos)
    {
        float aimAngle = GetMouseAngle(mousePos, HoldedItem);
        HoldedItem.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    private float GetMouseAngle(Vector2 mousePos, Transform refGameObject)
    {
        //Vector2 aimDiff = new Vector2(mousePos.x - refGameObject.position.x, mousePos.y - refGameObject.position.y);
        Vector2 aimDiff2 = (Vector3)mousePos - refGameObject.position;
        aimDiff2.Normalize();
        float aimAngle = Mathf.Atan2(aimDiff2.y, aimDiff2.x) * Mathf.Rad2Deg;
        return aimAngle;
    }

    private void RotateArm(Transform hand, Vector2 mousePos)
    {
        
    }

    public void RotateFlashlight(Vector2 mousePos)
    {
        float aimAngle = GetMouseAngle(mousePos, _flashlight.transform);
        _flashlight.transform.rotation = Quaternion.Euler(0, 0, aimAngle - 90f);
    }

    internal void ChangeFlashlightMode()
    {
        _flashlight.SetActive(!_flashlight.activeSelf);
    }
}
