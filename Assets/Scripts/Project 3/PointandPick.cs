using UnityEngine;
public class PointandPick : MonoBehaviour
{
    public OVRHand hand;
    public OVRSkeleton skeleton;
    public LayerMask surfaceMask; // 'Surface' layer from the MRUK code
    public Transform agentTarget;
    void Update()
    {
        if (!hand.IsTracked || hand.HandConfidence != OVRHand.TrackingConfidence.High) return;
        var wrist = skeleton.Bones[(int)OVRSkeleton.BoneId.XRHand_Wrist].Transform;
        var tip = skeleton.Bones[(int)OVRSkeleton.BoneId.XRHand_IndexTip].Transform;
        // Safer lookup but do not search the list every frame
        // Cache the Transform references after the skeleton is initialized:
        // var bone = skeleton.Bones.FirstOrDefault(b => b.Id == boneId);
        // var transform = bone?.Transform;
        Vector3 dir = (tip.position - wrist.position).normalized;
        if (Physics.Raycast(tip.position, dir, out RaycastHit hit, 10f, surfaceMask))
            agentTarget.position = hit.point;
        // OVRHand.PointerPose is more stable than wrist-to-index pointing
    }
}