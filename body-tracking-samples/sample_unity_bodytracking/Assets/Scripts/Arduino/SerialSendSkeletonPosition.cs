using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Kinect.BodyTracking;
using UnityEngine;

public class SerialSendSkeletonPosition : MonoBehaviour
{
    [SerializeField] private SkeletonJointPosition skeleton;
    [SerializeField] private JointId[] joints = { JointId.Head, JointId.HandLeft, JointId.HandRight, JointId.Pelvis };
    [SerializeField] private float sendInterval = 0.5f;

    public SerialHandler serialHandler;
    private double wL;
    private double wR;
    private double wP;

    void Start()
    {
        if (!skeleton) skeleton = FindObjectOfType<SkeletonJointPosition>();
        InvokeRepeating("Send", 0, sendInterval);
    }

    private void Send()
    {
        CalculateJointRotationAngles();
        serialHandler.Write(wL + "," + wR + "," + wP + "\n");
    }

    private void CalculateJointRotationAngles()
    {
        if (!skeleton) return;
        
        var frontVec = Vector3.Cross(skeleton.Get(JointId.ShoulderRight) - skeleton.Get(JointId.ShoulderLeft), Vector3.up).normalized;
        var worldToLocalRot = Quaternion.FromToRotation(frontVec, Vector3.forward);
        
        wL = GetJointLocalRotationAngle(JointId.ClavicleLeft, JointId.WristLeft, worldToLocalRot);
        wR = GetJointLocalRotationAngle(JointId.ClavicleRight, JointId.WristRight, worldToLocalRot);
        wP = GetJointLocalRotationAngle(JointId.Pelvis, JointId.Head, worldToLocalRot);
    }

    private double GetJointLocalRotationAngle(JointId joint, JointId child, Quaternion worldToLocalRot)
    {
        var dir = worldToLocalRot * (skeleton.Get(child) - skeleton.Get(joint));
        return Math.Atan2(dir.y, -dir.x) / 2 / Math.PI * 360;
    }

    private string FormatPosition(Vector3 pos)
    {
        return pos.x + "," + pos.y + "," + pos.z;
    }
}
