using System.Collections;
using TMPro;
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
    [SerializeField] GameObject checkpointNotif;
    [SerializeField] TextMeshProUGUI countdownText;

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
        if (!isRespawning && RaceManager.Instance.isGameStarted)
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
            if (other.gameObject != CheckpointSpawner.Instance.checkpoints[NextCheckpointIndex])
            {
                return;
            }
            StartCoroutine(CheckpointNotification());
            LastCheckpointPos = other.transform.position;
            other.gameObject.SetActive(false);
            NextCheckpointIndex++;
            if (NextCheckpointIndex >= CheckpointSpawner.Instance.checkpoints.Count)
            {
                waypointIndicator.target = null;
                RaceManager.Instance.isRaceComplete = true;
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
        checkpointNotif.SetActive(true);
        yield return new WaitForSeconds(1f);
        checkpointNotif.SetActive(false);
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
