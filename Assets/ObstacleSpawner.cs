using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform player;

    [Header("Префабы")]
    public GameObject triggerObstaclePrefab;
    public GameObject blockerObstaclePrefab;

    [Header("Спавн")]
    public float spawnAhead = 40f;
    public float despawnBehind = 15f;
    public float spawnIntervalZ = 6f;

    [Header("Сетка X")]
    public float[] laneX = { -4f, -1.5f, 0f, 1.5f, 4f };

    [Header("Сетка Y")]
    public float yMin = 0f;
    public float yMax = 2.5f;
    public int ySteps = 3;    // 0 = у пола, 1 = середина, 2 = верх (при ySteps=3)

    [Header("Плотность")]
    [Range(0f, 1f)] public float emptyRowChance = 0.2f;
    [Range(0f, 1f)] public float blockerChance = 0.4f;

    private float nextSpawnZ;
    private readonly List<GameObject> alive = new List<GameObject>();

    void Start()
    {
        nextSpawnZ = player.position.z + 10f;
    }

    void Update()
    {
        int safety = 0;
        while (nextSpawnZ < player.position.z + spawnAhead && safety < 50)
        {
            SpawnRow(nextSpawnZ);
            nextSpawnZ += spawnIntervalZ;
            safety++;
        }

        for (int i = alive.Count - 1; i >= 0; i--)
        {
            if (alive[i] == null) { alive.RemoveAt(i); continue; }
            if (alive[i].transform.position.z < player.position.z - despawnBehind)
            {
                Destroy(alive[i]);
                alive.RemoveAt(i);
            }
        }
    }

    void SpawnRow(float z)
    {
        if (Random.value < emptyRowChance) return;

        int count = Random.Range(1, 3);

        // подготовим ячейки: X из laneX, Y — дискретные шаги
        var cells = new List<(float x, float y)>();
        foreach (var x in laneX)
        {
            float y = PickY();
            cells.Add((x, y));
        }

        Shuffle(cells);

        for (int i = 0; i < count; i++)
        {
            var (x, y) = cells[i];
            Vector3 pos = new Vector3(x, y, z);

            GameObject prefab = Random.value < blockerChance
                ? blockerObstaclePrefab
                : triggerObstaclePrefab;

            var go = Instantiate(prefab, pos, Quaternion.identity, transform);
            alive.Add(go);
        }
    }

    float PickY()
    {
        if (ySteps <= 1) return Random.Range(yMin, yMax);

        // дискретные уровни между yMin и yMax
        int step = Random.Range(0, ySteps);
        float t = (float)step / (ySteps - 1);   // 0..1
        return Mathf.Lerp(yMin, yMax, t);
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}