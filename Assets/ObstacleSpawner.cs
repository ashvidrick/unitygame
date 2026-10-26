using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Ссылки")]
    public Transform player;

    [Header("Префабы")]
    public GameObject triggerPrefab;      // тип 1: взрыв + счёт
    public GameObject passThroughPrefab;  // тип 2: исчезает + счёт
    public GameObject blockerPrefab;      // тип 3: тормозит

    [Header("Спавн")]
    public float spawnAhead = 40f;
    public float despawnBehind = 15f;
    public float spawnIntervalZ = 5f;
    public float levelHeight = 3f;

    [Header("Сетка")]
    public float[] laneX = { -4f, -1.5f, 0f, 1.5f, 4f };
    public int[] levels = { -1, 0, 1 };

    [Header("Шансы появления (сумма не важна, Unity нормирует)")]
    [Range(0f, 1f)] public float emptyRowChance = 0.0f;
    [Range(0f, 1f)] public float triggerChance = 0.4f;
    [Range(0f, 1f)] public float passThroughChance = 0.3f;
    [Range(0f, 1f)] public float blockerChance = 0.3f;

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

        int count = Random.Range(1, 5);

        var cells = new List<(float x, int lvl)>();
        foreach (var x in laneX)
            foreach (var lvl in levels)
                cells.Add((x, lvl));

        Shuffle(cells);

        for (int i = 0; i < count; i++)
        {
            var (x, lvl) = cells[i];
            Vector3 pos = new Vector3(x, lvl * levelHeight, z);

            GameObject prefab = PickPrefab();
            if (prefab == null) continue;

            var go = Instantiate(prefab, pos, prefab.transform.rotation/*Quaternion.identity*/, transform);
            alive.Add(go);
        }
    }

    GameObject PickPrefab()
    {
        float total = triggerChance + passThroughChance + blockerChance;
        if (total <= 0f) return null;

        float r = Random.value * total;

        if (r < triggerChance) return triggerPrefab;
        r -= triggerChance;

        if (r < passThroughChance) return passThroughPrefab;
        r -= passThroughChance;

        return blockerPrefab;
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