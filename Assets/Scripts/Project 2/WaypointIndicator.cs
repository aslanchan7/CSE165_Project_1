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
        Vector3 playerPos = vrPlayer.position;
        Vector3 targetPos = target.transform.position;

        Vector3 pointDir = targetPos - playerPos;

        Quaternion worldRotToWaypoint = Quaternion.LookRotation(pointDir.normalized, playerCamera.up);
        Quaternion relativeRotation = Quaternion.Inverse(playerCamera.rotation) * worldRotToWaypoint;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, relativeRotation, rotationSpeed * Time.deltaTime);


        //Quaternion targetRot = Quaternion.LookRotation(pointDir, Vector3.up);

        //Quaternion targetRot = Quaternion.LookRotation(
        //    Camera.main.transform.InverseTransformDirection(pointDir),
        //    Vector3.up
        //);

        //waypointArrow.rotation = Quaternion.RotateTowards(waypointArrow.rotation, targetRot, rotationSpeed * Time.deltaTime);


        //Vector3 screenDir = Camera.main.WorldToScreenPoint(targetPos) - Camera.main.WorldToScreenPoint(playerPos);
        //float angle = Mathf.Atan2(screenDir.y, screenDir.x) * Mathf.Rad2Deg;
        //waypointArrow.rotation = Quaternion.Euler(0, 0, angle);
    }
}
