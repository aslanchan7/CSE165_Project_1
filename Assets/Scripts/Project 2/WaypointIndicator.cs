using UnityEngine;

public class WaypointIndicator : MonoBehaviour
{
    public Transform vrPlayer;
    public Transform playerCamera;
    public Transform waypointArrow;
    public GameObject target;
    public float rotationSpeed = 360f;
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            waypointArrow.gameObject.SetActive(false);
            return;
        }

        Vector3 playerPos = vrPlayer.position;
        Vector3 targetPos = target.transform.position;

        Vector3 pointDir = targetPos - playerPos;

        Quaternion worldRotToWaypoint = Quaternion.LookRotation(pointDir.normalized, playerCamera.up);
        Quaternion relativeRotation = Quaternion.Inverse(playerCamera.rotation) * worldRotToWaypoint;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, relativeRotation, rotationSpeed * Time.deltaTime);
    }
}
