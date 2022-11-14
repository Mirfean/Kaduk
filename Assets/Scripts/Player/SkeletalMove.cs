using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SkeletalMove : MonoBehaviour
{
    [SerializeField]
    private SpriteSkin skeleton;

    [SerializeField]
    public Transform leftHand;

    [SerializeField]
    private PlayerBasics player;

    [SerializeField]
    Animator animator;

    [SerializeField]
    bool IsAiming;

    [SerializeField]
    public Transform HoldedItem;

    [SerializeField]
    float HoldItemRotationIdle = -68f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

/*    private void OnAnimatorIK(int layerIndex)
    {
        if (IsAiming)
        {
            Vector2 aimDiff = new Vector2(mousePos.x - leftHandTracker.position.x, mousePos.y - leftHandTracker.position.y);
            float aimAngle = Mathf.Atan2(aimDiff.y, aimDiff.x) * Mathf.Rad2Deg;
            leftHandTracker.rotation = Quaternion.Euler(0, 0, aimAngle);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTracker.rotation);

            aimDiff = new Vector2(mousePos.x - rightHandTracker.position.x, mousePos.y - rightHandTracker.position.y);
            aimAngle = Mathf.Atan2(aimDiff.y, aimDiff.x) * Mathf.Rad2Deg;
            rightHandTracker.rotation = Quaternion.Euler(0, 0, aimAngle);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTracker.rotation);
        }
    }*/

    /// <summary>
    /// Setting HoldItem to Idle basic status(Z rotation to -68)
    /// </summary>
    public void SetArmsToIdle()
    {
        HoldedItem.rotation = Quaternion.Euler(0, 0, HoldItemRotationIdle);
    }

    public void TrackCursorByHands(Vector2 mousePos)
    {
        Vector2 aimDiff = new Vector2(mousePos.x - HoldedItem.position.x, mousePos.y - HoldedItem.position.y);
        float aimAngle = Mathf.Atan2(aimDiff.y, aimDiff.x) * Mathf.Rad2Deg;
        HoldedItem.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    private void RotateArm(Transform hand, Vector2 mousePos)
    {
        
    }
}
