using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Gestures;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class HandController : MonoBehaviour
{

    [Header("Hand Tracking Settings")]
    [SerializeField] XRHandTrackingEvents leftHandTrackingEvents;
    [SerializeField] XRHandTrackingEvents rightHandTrackingEvents;
    [SerializeField] XRHandShape[] handShapes;
    [SerializeField] float gestureDetectionInterval = 0.1f;
    [SerializeField] float minGestureThreshold = 0.9f;
    [SerializeField] HandShapeCompletenessCalculator handShapeCompletenessCalculator;

    [Header("References")]
    [SerializeField] DroneController droneController;

    // Private variables
    private XRHandSubsystem m_HandSubsystem;
    private float timeOfLastGestureDetection;

    void Start()
    {
        var handSubsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(handSubsystems);

        for (var i = 0; i < handSubsystems.Count; ++i)
        {
            var handSubsystem = handSubsystems[i];
            if (handSubsystem.running)
            {
                m_HandSubsystem = handSubsystem;
                break;
            }
        }

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
            Debug.Log($"Index rotation: {pose.rotation.eulerAngles}");

            droneController.MoveDir = pose.rotation * Vector3.forward;
        }

        if(Time.time - timeOfLastGestureDetection > gestureDetectionInterval)
        {
            foreach (var handShape in handShapes)
            {
                handShapeCompletenessCalculator.TryCalculateHandShapeCompletenessScore(args.hand, handShape, out float completenessScore);
                if (completenessScore > minGestureThreshold)
                {
                    Debug.Log($"Detected gesture: {handShape.name} with completeness: {completenessScore}");
                    // Perform actions based on the detected gesture
                    droneController.MoveDir = Vector3.zero;
                }
            }

            timeOfLastGestureDetection = Time.time;            
        }
        
    }
}

