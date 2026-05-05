using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectSpawnerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform gridParent; 
    [SerializeField] Transform equipmentSpawnPoint; 
    
    [Header("Prefabs")]
    [SerializeField] List<GameObject> equipmentPrefabs = new();
    [SerializeField] GameObject buttonPrefab;
    
    [Header("Config")]
    [SerializeField] int buttonCount;

    void Start()
    {
        for (int i = 0; i < buttonCount; i++)
        {
            int index = i;
            GameObject instantiated = Instantiate(buttonPrefab, gridParent);
            instantiated.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            instantiated.GetComponent<Button>().onClick.AddListener(() =>
            {
                SpawnObject(index);
            });
        }
    }

    void SpawnObject(int index)
    {
        GameObject instantiated = Instantiate(equipmentPrefabs[index]);
        instantiated.transform.position = equipmentSpawnPoint.position;
        instantiated.AddComponent<BoxCollider>();
        instantiated.AddComponent<Rigidbody>();
        instantiated.AddComponent<XRGrabInteractable>();
    }
}
