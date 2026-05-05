using System.Collections.Generic;
using UnityEngine;

public class CheckpointSpawner : MonoBehaviour
{
    public TextAsset file;

    public GameObject checkpointPrefab;

    private List<Vector3> checkpointPositions = new List<Vector3>();


    void Start()
    {
        if (file != null) 
        {
            checkpointPositions = ParseFile();
            foreach (Vector3 vec in checkpointPositions)
            {
                Instantiate(checkpointPrefab, vec, Quaternion.identity);
            }
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
