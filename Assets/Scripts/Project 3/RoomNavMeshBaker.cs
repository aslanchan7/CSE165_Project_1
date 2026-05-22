using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Meta.XR.MRUtilityKit;

public class RoomNavMeshBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;

    void OnEnable()
    {
        MRUK.Instance.SceneLoadedEvent.AddListener(OnSceneLoaded);
    }

    void OnDisable()
    {
        MRUK.Instance.SceneLoadedEvent.RemoveListener(OnSceneLoaded);
    }

    void OnSceneLoaded()
    {
        var room = MRUK.Instance.GetCurrentRoom();

        // Set floor walkable
        SetupAnchorForNavMesh(room.FloorAnchor, true);

        // Set walls as obstacles (non-walkable)
        foreach (var wall in room.WallAnchors)
        {
            wall.gameObject.layer = LayerMask.NameToLayer("Surface");
            SetupAnchorForNavMesh(wall, false);
        }

        // Optional: mark furniture/volumes as obstacles
        foreach (var anchor in room.Anchors)
        {
            if (anchor == room.FloorAnchor) continue;
            SetupAnchorForNavMesh(anchor, false);
        }

        BakeNavMesh();
    }

    void SetupAnchorForNavMesh(MRUKAnchor anchor, bool walkable)
    {
        if (anchor == null) return;

        var modifier = anchor.gameObject.GetComponent<NavMeshModifier>();
        if (modifier == null)
            modifier = anchor.gameObject.AddComponent<NavMeshModifier>();

        modifier.overrideArea = true;
        modifier.area = walkable
            ? NavMesh.GetAreaFromName("Walkable")
            : NavMesh.GetAreaFromName("Not Walkable");
    }

    void BakeNavMesh()
    {
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface not assigned!");
            return;
        }

        // Collect geometry from the scene objects
        navMeshSurface.collectObjects = CollectObjects.All;
        navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;

        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh baked successfully.");
    }
}
