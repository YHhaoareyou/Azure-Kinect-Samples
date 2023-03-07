using Microsoft.Azure.Kinect.BodyTracking;
using UnityEngine;

public class SkeletonJointPosition : MonoBehaviour
{
    [SerializeField] private Transform skeletonRoot;

    private void Awake()
    {
        if (!skeletonRoot) skeletonRoot = transform;
    }

    public Vector3 Get(JointId jointId)
    {
        return transform.GetChild((int)jointId).localPosition;
    }
}
