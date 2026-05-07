using System.Collections;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [HideInInspector] public Vector3 MoveDir;
    [HideInInspector] public Vector3 LastCheckpointPos;
    [HideInInspector] public int NextCheckpointIndex;

    [Header("References")]
    [SerializeField] Transform vrPlayer;
    [SerializeField] Transform droneModel;
    [SerializeField] WaypointIndicator waypointIndicator;

    [Header("Movement Config")]
    [SerializeField] float moveSpeed = 10f;

    private bool isRespawning = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveDir = Vector3.zero; 
        LastCheckpointPos = transform.position;
        NextCheckpointIndex = 0;
        waypointIndicator.target = CheckpointSpawner.Instance.checkpoints[NextCheckpointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRespawning)
        {
            MoveDir = MoveDir.normalized;
            vrPlayer.position += moveSpeed * Time.deltaTime * MoveDir;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            // TODO: Reset drone position to last checkpoint, rotate drone such that it points towards the next checkpoint, and reset drone velocity to zero, start countdown timer for respawn
            Debug.Log("Ground Hit");
            vrPlayer.position = LastCheckpointPos;
            MoveDir = Vector3.zero;
            StartCoroutine(RespawnCountdown());
        }
        else if (other.CompareTag("Checkpoint"))
        {
            // TODO: Update last checkpoint position, and update next checkpoint position
            Debug.Log("Checkpoint reached!");
            if (other.gameObject != CheckpointSpawner.Instance.checkpoints[NextCheckpointIndex] ||
                NextCheckpointIndex >= CheckpointSpawner.Instance.checkpoints.Count)
            {
                return;
            }
            LastCheckpointPos = other.transform.position;
            other.gameObject.SetActive(false);
            NextCheckpointIndex++;
            if (NextCheckpointIndex >= CheckpointSpawner.Instance.checkpoints.Count)
            {
                return;
            }

            // Update References to Next Checkpoint
            CheckpointSpawner.Instance.checkpoints[NextCheckpointIndex].GetComponent<Renderer>().material.color = Color.red;
            waypointIndicator.target = CheckpointSpawner.Instance.checkpoints[NextCheckpointIndex];
        }
    }

     IEnumerator RespawnCountdown()
    {
        isRespawning = true;
        yield return new WaitForSeconds(3f);
        isRespawning = false;
    }
}
