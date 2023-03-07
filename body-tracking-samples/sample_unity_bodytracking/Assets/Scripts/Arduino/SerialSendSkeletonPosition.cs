using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Kinect.BodyTracking;
using UnityEngine;

public class SerialSendSkeletonPosition : MonoBehaviour
{
    [SerializeField] private SkeletonJointPosition skeletonJointPosition;
    [SerializeField] private JointId[] joints = { JointId.Head, JointId.HandLeft, JointId.HandRight, JointId.Pelvis };
    [SerializeField] private float sendInterval = 0.5f;
    public SerialHandler serialHandler;

    void Start()
    {
        InvokeRepeating("Send", 0, sendInterval);
    }

    private void Send()
    {
        serialHandler.Write(String.Join("|", joints.Select(jointId => 
            FormatPosition(skeletonJointPosition.Get(jointId)))) + "\n");
    }

    private string FormatPosition(Vector3 pos)
    {
        return pos.x + "," + pos.y + "," + pos.z;
    }
}
