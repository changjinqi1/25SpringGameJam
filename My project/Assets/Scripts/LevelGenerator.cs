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

        if (!firstLevelSpawned)
        {
            prefabToSpawn = startLevelPrefab;
            firstLevelSpawned = true;

            GameObject levelInstance = Instantiate(prefabToSpawn, currentSpawnPosition, Quaternion.identity);

            // 定位关键点
            Transform startPoint = levelInstance.transform.Find("StartPoint");
            Transform stickPoint = levelInstance.transform.Find("StickPoint");

            // 设置玩家起始位置
            player.position = startPoint.position;
            player.GetComponent<PlayerOrbit>().stick = stickPoint;

            lastStickPoint = stickPoint;
        }
        else
        {
            if (currentLevelPool.Count == 0)
            {
                ShuffleNewRound();
            }

            prefabToSpawn = currentLevelPool.Dequeue();

            // 用 StickPoint 作为对齐位置
            GameObject levelInstance = Instantiate(prefabToSpawn);
            Transform newStartPoint = levelInstance.transform.Find("StartPoint");
            Transform newStickPoint = levelInstance.transform.Find("StickPoint");

            if (newStartPoint != null && lastStickPoint != null)
            {
                Vector3 offset = lastStickPoint.position - newStartPoint.position;
                levelInstance.transform.position += offset;
            }

            player.GetComponent<PlayerOrbit>().stick = newStickPoint;
            lastStickPoint = newStickPoint;
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
}
