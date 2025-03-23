using System.Collections;
using UnityEngine;

public class YarnBallRespawner : MonoBehaviour
{
    public GameObject yarnBallPrefab;  // 毛球预制体
    public float respawnDelay = 3f;    // 延迟时间

    private Vector3 spawnPosition;
    private bool collected = false;

    void Start()
    {
        spawnPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;

            // Disable current ball
            gameObject.SetActive(false);

            // Start respawn coroutine
            StartCoroutine(RespawnYarnBall());
        }
    }

    IEnumerator RespawnYarnBall()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Instantiate a new ball at the same position
        GameObject newBall = Instantiate(yarnBallPrefab, spawnPosition, Quaternion.identity);

        // Optional: Attach the respawner script to new ball
        newBall.AddComponent<YarnBallRespawner>().yarnBallPrefab = yarnBallPrefab;
    }
}
