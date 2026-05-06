using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

public class HandController : MonoBehaviour
{
    XRHandSubsystem m_HandSubsystem;

    public GameObject droneObject;
    public float moveSpeed = 10.0f;

    void Start()
    {
        var handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(handSubsystems);

        Debug.Log($"Found {handSubsystems.Count} hand subsystems.");

        for (var i = 0; i < handSubsystems.Count; ++i)
        {
            var handSubsystem = handSubsystems[i];
            if (handSubsystem.running)
            {
                m_HandSubsystem = handSubsystem;
                break;
            }
        }

        Debug.Log(m_HandSubsystem);

        if (m_HandSubsystem != null)
            m_HandSubsystem.updatedHands += OnUpdatedHands;
    }

    void OnUpdatedHands(XRHandSubsystem subsystem,
        XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags,
        XRHandSubsystem.UpdateType updateType)
    {

        switch (updateType)
        {
            case XRHandSubsystem.UpdateType.Dynamic:
                // Update game logic that uses hand data
                Debug.Log("Dynamic");
                break;
            case XRHandSubsystem.UpdateType.BeforeRender:
                // Update visual objects that use hand data
                Debug.Log("Before Render");
                break;
        }
    }

    public void OnJointsUpdated(XRHandJointsUpdatedEventArgs args)
    {
        if (args.hand.GetJoint(XRHandJointID.IndexTip).TryGetPose(out var pose))
        {
            Debug.Log($"Wrist position: {pose.position}");
            Debug.Log($"Wrist rotation: {pose.rotation.eulerAngles}");

            Vector3 moveDir = pose.rotation * Vector3.forward;
            moveDir = moveDir.normalized;

            droneObject.transform.position += moveDir * Time.deltaTime * moveSpeed;
        }
    }
}

