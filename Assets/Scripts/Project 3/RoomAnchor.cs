using UnityEngine;
using System.Threading.Tasks;
public class RoomAnchor : MonoBehaviour
{
    public GameObject planePrefab; // Quad with a Mesh Collider
    public async Task CreateAnchor(Vector3 pos, Quaternion rot, Vector2 size)
    {
        var plane = Instantiate(planePrefab, pos, rot);
        plane.transform.localScale = new Vector3(size.x, size.y, 0.01f);
        var anchor = plane.AddComponent<OVRSpatialAnchor>();
        while (!anchor.Created || !anchor.Localized) await Task.Yield();
        await anchor.SaveAnchorAsync(); // optional: persist
    }

    public async void SpawnAnchor()
    {
        await CreateAnchor(transform.position, transform.rotation, new Vector2(1f, 1f));
    }
}