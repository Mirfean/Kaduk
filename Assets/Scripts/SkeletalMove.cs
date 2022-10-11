using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SkeletalMove : MonoBehaviour
{
    [SerializeField]
    private SpriteSkin skeleton;

    [SerializeField]
    private Transform leftHandTracker;

    [SerializeField]
    private Transform rightHandTracker;

    [SerializeField]
    private PlayerBasics player;

    [SerializeField]
    Animator animator;

    [SerializeField]
    bool IsAiming;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
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
    }

    public void TrackCursorByHands(Vector2 mousePos)
    {
        
    }

    private void RotateArm(Transform hand, Vector2 mousePos)
    {
        Vector2 aimDiff = new Vector2(mousePos.x - hand.position.x, mousePos.y - hand.position.y);
        float aimAngle = Mathf.Atan2(aimDiff.y, aimDiff.x) * Mathf.Rad2Deg;
        hand.rotation = Quaternion.Euler(0, 0, aimAngle);
    }
}
