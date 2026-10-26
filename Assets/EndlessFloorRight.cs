using UnityEngine;

public class EndlessFloorRight : MonoBehaviour
{
    public Transform player;

    [Header("Префаб 1")]
    public GameObject floorPrefab1;
    [Header("Префаб 2")]
    public GameObject floorPrefab2;
    [Header("Префаб 3")]
    public GameObject floorPrefab3;
    [Header("Префаб 4")]
    public GameObject floorPrefab4;

    public int visibleSegments = 6;
    public float segmentLength = 10f;

    private Transform[] segments1;
    private Transform[] segments2;
    private Transform[] segments3;
    private Transform[] segments4;

    void Start()
    {
        segments1 = new Transform[visibleSegments];
        segments2 = new Transform[visibleSegments];
        segments3 = new Transform[visibleSegments];
        segments4 = new Transform[visibleSegments];
        for (int i = 0; i < visibleSegments; i++)
        {
            var go1 = Instantiate(floorPrefab1, transform);
            var go2 = Instantiate(floorPrefab2, transform);
            var go3 = Instantiate(floorPrefab3, transform);
            var go4 = Instantiate(floorPrefab4, transform);
            
            go1.transform.position = new Vector3(10f, 10f, i * segmentLength);
            segments1[i] = go1.transform;
            
            go2.transform.position = new Vector3(-10f, -10f, i * segmentLength);
            segments2[i] = go2.transform;

            go3.transform.position = new Vector3(-10f, 10f, i * segmentLength);
            segments3[i] = go3.transform;

            go4.transform.position = new Vector3(10f, -10f, i * segmentLength);
            segments4[i] = go4.transform;
        }
    }

    void Update()
    {
        for (int i = 0; i < segments1.Length; i++)
        {
            // если сегмент ушёл за спину игрока на одну длину — переносим вперёд
            if (segments1[i].position.z < player.position.z - segmentLength)
            {
                float maxZ = GetMaxZ();
                segments1[i].position = new Vector3(10f, 10f, maxZ + segmentLength);
            }
            
            // если сегмент ушёл за спину игрока на одну длину — переносим вперёд
            if (segments2[i].position.z < player.position.z - segmentLength)
            {
                float maxZ = GetMaxZ();
                segments2[i].position = new Vector3(-10f, -10f, maxZ + segmentLength);
            }

            // если сегмент ушёл за спину игрока на одну длину — переносим вперёд
            if (segments3[i].position.z < player.position.z - segmentLength)
            {
                float maxZ = GetMaxZ();
                segments3[i].position = new Vector3(-10f, 10f, maxZ + segmentLength);
            }

            if (segments4[i].position.z < player.position.z - segmentLength)
            {
                float maxZ = GetMaxZ();
                segments4[i].position = new Vector3(10f, -10f, maxZ + segmentLength);
            }
        }
    }

    float GetMaxZ()
    {
        float max = float.MinValue;
        foreach (var s in segments1)
            if (s.position.z > max) max = s.position.z;
        return max;
    }
}