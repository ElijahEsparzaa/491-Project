using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBone : MonoBehaviour
{
    public Transform boneToFollow;  // drag in the hand bone (e.g., bone_12)
    public Vector3 offset;
    public bool matchRotation = true;

    void LateUpdate()
{
    if (boneToFollow != null)
    {
        //Follow bone with a Local offset so it moves naturally with the hand
        transform.position = boneToFollow.TransformPoint(offset);

        if (matchRotation)
            transform.rotation = boneToFollow.rotation;

        //Handle flipping
        Vector3 localScale = transform.localScale;
        localScale.x = Mathf.Sign(boneToFollow.lossyScale.x) * Mathf.Abs(localScale.x);
        transform.localScale = localScale;
    }
}
}