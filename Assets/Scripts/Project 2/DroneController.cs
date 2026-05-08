using System.Collections;
using TMPro;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    [HideInInspector] public Vector3 MoveDir;
    [HideInInspector] public Vector3 LastCheckpointPos;
    [HideInInspector] public int NextCheckpointIndex;

    [Header("References")]
    [SerializeField] public Transform vrPlayer;
    [SerializeField] Transform droneModel;
    [SerializeField] WaypointIndicator waypointIndicator;
    [SerializeField] TextMeshProUGUI checkpointNotif;
    [SerializeField] TextMeshProUGUI countdownText;

    [Header("Movement Config")]
    [SerializeField] float moveSpeed = 10f;

    [Header("3rd Person View Config")]
    public bool is3rdPersonView = false;
    public float orbitRadius = 4f;

    private Vector3 origDroneOffset;
    private Vector3 lastDroneOffset;

    private bool isRespawning = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoveDir = Vector3.zero; 
        LastCheckpointPos = transform.position;
        NextCheckpointIndex = 0;
        waypointIndicator.target = CheckpointSpawner.Instance.checkpoints[NextCheckpointIndex];
        origDroneOffset = droneModel.localPosition;
        lastDroneOffset = new Vector3(origDroneOffset.x, 0, origDroneOffset.z);
        Debug.Log(origDroneOffset);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRespawning && RaceManager.Instance.isGameStarted)
        {
            MoveDir = MoveDir.normalized;
            vrPlayer.position += moveSpeed * Time.deltaTime * MoveDir;
        }
        if (is3rdPersonView && MoveDir != Vector3.zero)
        {
            Vector3 worldMoveDir = vrPlayer.transform.TransformDirection(MoveDir);
            Vector3 offsetDir = new Vector3(worldMoveDir.x, 0f, worldMoveDir.z).normalized;
            Vector3 targetPos = new Vector3(
                offsetDir.x * orbitRadius,
                0f,
                offsetDir.z * orbitRadius
            );
            droneModel.localPosition = Vector3.Lerp(droneModel.localPosition, targetPos, 5f * Time.deltaTime);
            lastDroneOffset = droneModel.localPosition;
        }
        else if (is3rdPersonView && MoveDir == Vector3.zero)
        {
            droneModel.localPosition = Vector3.Lerp(droneModel.localPosition, lastDroneOffset, 5f * Time.deltaTime);
        }
        else
        {
            droneModel.localPosition = origDroneOffset;
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
            if (other.gameObject != CheckpointSpawner.Instance.checkpoints[NextCheckpointIndex])
            {
                return;
            }
            StartCoroutine(CheckpointNotification());
            LastCheckpointPos = other.transform.position;
            other.gameObject.SetActive(false);
            NextCheckpointIndex++;
            // Handles Race Finish
            if (NextCheckpointIndex >= CheckpointSpawner.Instance.checkpoints.Count)
            {
                waypointIndicator.target = null;
                RaceManager.Instance.isRaceComplete = true;
                checkpointNotif.text = "Race Finish!";
                StartCoroutine(CheckpointNotification());
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
        int countdown = 3;
        countdownText.gameObject.SetActive(true);
        countdownText.text = countdown.ToString();
        while (countdown > 0)
        {
            yield return new WaitForSeconds(1f);
            countdown--;
            countdownText.text = countdown.ToString();
        }
        countdownText.gameObject.SetActive(false);
        isRespawning = false;
    }

    IEnumerator CheckpointNotification()
    {
        checkpointNotif.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        checkpointNotif.gameObject.SetActive(false);
    }

    public IEnumerator GameCountdown()
    {
        int countdown = 3;
        countdownText.gameObject.SetActive(true);
        countdownText.text = countdown.ToString();
        while (countdown > 0)
        {
            yield return new WaitForSeconds(1f);
            countdown--;
            countdownText.text = countdown.ToString();
        }
        RaceManager.Instance.isGameStarted = true;
        countdownText.gameObject.SetActive(false);
    }
}
