using UnityEngine;

public class EndlessFloorRight : MonoBehaviour
{
    public Transform player;
    public GameObject floorPrefab;
    public int visibleSegments = 6;
    public float segmentLength = 10f;

    private Transform[] segments;

    void Start()
    {
        segments = new Transform[visibleSegments];
        for (int i = 0; i < visibleSegments; i++)
        {
            var go = Instantiate(floorPrefab, transform);
            go.transform.position = new Vector3(5f, 0f, i * segmentLength);
            segments[i] = go.transform;
        }
    }

    void Update()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            // если сегмент ушёл за спину игрока на одну длину — переносим вперёд
            if (segments[i].position.z < player.position.z - segmentLength)
            {
                float maxZ = GetMaxZ();
                segments[i].position = new Vector3(5f, 0f, maxZ + segmentLength);
            }
        }
    }

    float GetMaxZ()
    {
        float max = float.MinValue;
        foreach (var s in segments)
            if (s.position.z > max) max = s.position.z;
        return max;
    }
}