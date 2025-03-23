using System.Collections; // 确保包含此命名空间
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollowUpOnly : MonoBehaviour
{
    public Transform player;
    public Transform stick;
    public float radius = 5f;
    public float yOffset = 3f;
    public string nextSceneName;
    public AudioSource deathSound;

    private bool cameraReady = false;
    private float lastPlayerY;

    void Start()
    {
        if (player == null || stick == null)
        {
            Debug.LogError("Player or Stick not assigned!");
        }

        lastPlayerY = player.position.y;
    }

    void LateUpdate()
    {
        if (player == null || stick == null) return;

        // 1️⃣ Orbit position (horizontal)
        Vector3 playerOffset = player.position - stick.position;
        float playerAngle = Mathf.Atan2(playerOffset.z, playerOffset.x);
        Vector3 offset = new Vector3(Mathf.Cos(playerAngle), 0, Mathf.Sin(playerAngle)) * radius;
        Vector3 targetXZ = stick.position + offset;

        // 2️⃣ Y-axis: only move up
        float currentTargetY = transform.position.y;
        float desiredY = player.position.y + yOffset;

        if (desiredY > currentTargetY || !cameraReady)
        {
            currentTargetY = desiredY;
        }

        // Enable camera tracking after first snap
        if (!cameraReady)
        {
            cameraReady = true;
        }

        // Apply position
        transform.position = new Vector3(targetXZ.x, currentTargetY, targetXZ.z);
        lastPlayerY = player.position.y;

        // 3️⃣ Rotate to face stick
        Vector3 lookDir = stick.position - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
        }

        // 4️⃣ Death check: player fell off screen
        if (cameraReady)
        {
            Vector3 playerViewportPos = Camera.main.WorldToViewportPoint(player.position);
            if (playerViewportPos.y < 0f)
            {
                HandlePlayerDeath();
            }
        }
    }

    void HandlePlayerDeath()
    {
        Debug.Log("Player Dead! Playing death sound and loading next scene...");

        if (deathSound != null)
        {
            deathSound.Play();
        }

        PlayerPrefs.SetFloat("LastScore", player.position.y);

        StartCoroutine(DelayAndLoadNextScene(2f)); // 使用协程延迟 2 秒加载下一场景
    }

    private IEnumerator DelayAndLoadNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadNextScene();
    }

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("Next scene name not set in Inspector!");
        }
    }
}