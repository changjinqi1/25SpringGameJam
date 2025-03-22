using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject startLevelPrefab;
    public List<GameObject> randomLevelPrefabs;

    [Header("Generation Settings")]
    public Transform player;              
    public float levelHeight = 5f;        // Height of each level prefab
    public int preloadCount = 2;          // level number

    private Vector3 currentSpawnPosition;
    private bool firstLevelSpawned = false;
    private Queue<GameObject> currentLevelPool = new Queue<GameObject>();
    private List<GameObject> levelBuffer = new List<GameObject>();
    private int levelsGenerated = 0;

    void Start()
    {
        currentSpawnPosition = Vector3.zero;

        // generate start prefab
        SpawnNextLevel(); 
        for (int i = 0; i < preloadCount; i++)
        {
            SpawnNextLevel(); 
        }
    }

    void Update()
    {
        //generate next level
        if (player.position.y + 10f >= currentSpawnPosition.y)
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

            // Find StartPoint and StickPoint in scene
            Transform startPoint = levelInstance.transform.Find("StartPoint");
            Transform stickPoint = levelInstance.transform.Find("StickPoint");

            // set player pos, bound scene
            player.position = startPoint.position;
            player.GetComponent<PlayerOrbitWithGravityAndCollision>().stick = stickPoint;
        }
        else
        {
            // next round generation
            if (currentLevelPool.Count == 0)
            {
                ShuffleNewRound();
            }

            prefabToSpawn = currentLevelPool.Dequeue();
            GameObject levelInstance = Instantiate(prefabToSpawn, currentSpawnPosition, Quaternion.identity);

            Transform stickPoint = levelInstance.transform.Find("StickPoint");
            player.GetComponent<PlayerOrbitWithGravityAndCollision>().stick = stickPoint;
        }

   
        currentSpawnPosition += new Vector3(0, levelHeight, 0); // Stack Up
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
