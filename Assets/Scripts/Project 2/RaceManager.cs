using TMPro;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance;

    public bool isGameStarted = false;
    public bool isRaceComplete = false;
    private float elapsedTime = 0f;
    [SerializeField] private TextMeshProUGUI timeText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isGameStarted = false;
        isRaceComplete = false;
        timeText.text = string.Format("{0:00}:{1:00}", 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStarted || isRaceComplete)
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        int min = Mathf.FloorToInt(elapsedTime / 60f);
        int sec = Mathf.FloorToInt(elapsedTime % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
