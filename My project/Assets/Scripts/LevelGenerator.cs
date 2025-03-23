using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject startLevelPrefab;
    public List<GameObject> randomLevelPrefabs;

    [Header("Generation Settings")]
    public Transform player;
    public int preloadCount = 2;

    private Vector3 currentSpawnPosition;
    private bool firstLevelSpawned = false;
    private Queue<GameObject> currentLevelPool = new Queue<GameObject>();
    private List<GameObject> levelBuffer = new List<GameObject>();
    private int levelsGenerated = 0;
    private Transform lastStickPoint;
    private List<GameObject> last3Levels = new List<GameObject>();


    void Start()
    {
        currentSpawnPosition = Vector3.zero;

        SpawnNextLevel();
        for (int i = 0; i < preloadCount; i++)
        {
            SpawnNextLevel();
        }
    }
    public void SpawnNextLevel()
    {
        GameObject prefabToSpawn;

        // 固定高度单位
        float levelHeight = 2 * 9.28408f;
        if (!firstLevelSpawned)
        {
            prefabToSpawn = startLevelPrefab;
            firstLevelSpawned = true;

            Vector3 spawnPos = new Vector3(0, 9.28408f, 0); // ✅ 向上偏移半个高度
            GameObject levelInstance = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            Transform startPoint = levelInstance.transform.Find("StartPoint");
            Transform stickPoint = levelInstance.transform.Find("StickPoint");

            if (startPoint != null) player.position = startPoint.position;
            if (stickPoint != null) player.GetComponent<PlayerOrbit>().stick = stickPoint;

            lastStickPoint = stickPoint;
        }

        else
        {
            if (currentLevelPool.Count == 0)
            {
                ShuffleNewRound();
            }

            // 选一个未在 last3Levels 中出现的 prefab
            prefabToSpawn = GetNextValidLevel();

            // 计算生成位置
            float spawnY = 9.28408f + levelsGenerated * levelHeight;
            Vector3 spawnPos = new Vector3(0, spawnY, 0);

            GameObject levelInstance = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

            // StickPoint 若仍用于玩家粘附点则保留
            Transform newStickPoint = levelInstance.transform.Find("StickPoint");
            if (newStickPoint != null)
            {
                player.GetComponent<PlayerOrbit>().stick = newStickPoint;
                lastStickPoint = newStickPoint;
            }
        }

        levelsGenerated++;
    }


    void ShuffleNewRound()
    {
        levelBuffer.Clear();
        levelBuffer.AddRange(randomLevelPrefabs);

        for (int i = 0; i < levelBuffer.Count; i++)
        {
            GameObject temp = levelBuffer[i];
            int randomIndex = Random.Range(i, levelBuffer.Count);
            levelBuffer[i] = levelBuffer[randomIndex];
            levelBuffer[randomIndex] = temp;
        }

        foreach (GameObject level in levelBuffer)
        {
            currentLevelPool.Enqueue(level);
        }
    }
    private GameObject GetNextValidLevel()
    {
        GameObject prefabToSpawn = null;
        int safety = 20;

        while (safety-- > 0 && currentLevelPool.Count > 0)
        {
            GameObject candidate = currentLevelPool.Dequeue();
            if (!last3Levels.Contains(candidate))
            {
                prefabToSpawn = candidate;
                last3Levels.Add(candidate);
                if (last3Levels.Count > 3)
                    last3Levels.RemoveAt(0);
                break;
            }
            else
            {
                currentLevelPool.Enqueue(candidate); // 放回末尾
            }
        }

        // 如果实在找不到，就强制取一个
        if (prefabToSpawn == null && currentLevelPool.Count > 0)
        {
            prefabToSpawn = currentLevelPool.Dequeue();
            last3Levels.Add(prefabToSpawn);
            if (last3Levels.Count > 3)
                last3Levels.RemoveAt(0);
        }

        return prefabToSpawn;
    }

}
