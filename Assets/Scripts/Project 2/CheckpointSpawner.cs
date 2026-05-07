using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckpointSpawner : MonoBehaviour
{
    public static CheckpointSpawner Instance;

    [Header("References")]
    public TextAsset file;
    public GameObject checkpointPrefab;

    public List<GameObject> checkpoints = new List<GameObject>();
    private List<Vector3> checkpointPositions = new List<Vector3>();

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

        if (file != null) 
        {
            checkpointPositions = ParseFile();
            foreach (Vector3 vec in checkpointPositions)
            {
                GameObject checkpoint = Instantiate(checkpointPrefab, vec, Quaternion.identity);
                checkpoints.Add(checkpoint);
            }
            checkpoints[0].GetComponent<Renderer>().material.color = Color.red;
        }
    }

    List<Vector3> ParseFile()
    {
        float ScaleFactor = 1.0f / 39.37f;
        List<Vector3> positions = new List<Vector3>();
        string content = file.ToString();
        string[] lines = content.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] coords = lines[i].Split(' ');
            Vector3 pos = new Vector3(float.Parse(coords[0]), float.Parse(coords[1]), float.Parse(coords[2]));
            positions.Add(pos * ScaleFactor);
        }
        return positions;
    }
}
