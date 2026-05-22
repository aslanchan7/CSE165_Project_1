using Oculus.Interaction.Input;
using UnityEngine;

public class StupidDebugScript : MonoBehaviour
{
    [SerializeField] private HandRef handRef;
    [SerializeField] private Rigidbody agentRb;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Animator anim;
    bool isPoseActive = false;
    private Pose pose;

    public void PoseActivated()
    {
        Debug.Log("Pose Activated");
        isPoseActive = true;
    }

    public void PoseDeactivated()
    {
        Debug.Log("Pose Deactivated");
        isPoseActive = false;
    }

    public void Reset()
    {
        agentRb.transform.localPosition = new Vector3(0f,0f,1f);
    }


    void Start()
    {
        agentRb.transform.localPosition = new(agentRb.transform.localPosition.x, 0f, agentRb.transform.localPosition.z);
    }

    void Update()
    {
        //if (indexTip == null) return;
        if (!isPoseActive)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }
        handRef.Hand.GetJointPose(HandJointId.HandIndexTip, out Pose pose);
        Debug.Log($"Index rotation: {pose.rotation.eulerAngles}");
        Vector3 worldDir = pose.rotation * Vector3.forward;
        worldDir.y = 0; // Keep movement on the horizontal plane
        //agentRb.linearVelocity = moveSpeed * worldDir;
        agentRb.transform.localPosition += moveSpeed * Time.deltaTime * worldDir;
        agentRb.transform.rotation = Quaternion.LookRotation(worldDir);
        anim.SetFloat("Speed", agentRb.linearVelocity.magnitude);

        //Vector3 origin = indexTip.position;

        //// Usually works well for pointing
        //Vector3 direction = indexTip.forward;

        //Debug.DrawRay(origin, direction * 0.5f, Color.red);
    }
}
