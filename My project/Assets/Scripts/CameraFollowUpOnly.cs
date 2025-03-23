using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollowUpOnly : MonoBehaviour
{
    public Transform player;
    public Transform stick;
    public float radius = 5f;
    public float yOffset = 3f;
    public float smoothSpeed = 5f;
    public string nextSceneName; // 🎯 场景名在 Inspector 设置

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

        // 1️⃣ 同步角度
        Vector3 playerOffset = player.position - stick.position;
        float playerAngle = Mathf.Atan2(playerOffset.z, playerOffset.x);
        Vector3 offset = new Vector3(Mathf.Cos(playerAngle), 0, Mathf.Sin(playerAngle)) * radius;
        Vector3 targetXZ = stick.position + offset;

        // 2️⃣ Y轴向上跟随
        float targetY = transform.position.y;
        if (player.position.y > lastPlayerY)
        {
            targetY = Mathf.Lerp(transform.position.y, player.position.y + yOffset, smoothSpeed * Time.deltaTime);
        }
        lastPlayerY = player.position.y;

        transform.position = new Vector3(targetXZ.x, targetY, targetXZ.z);

        // 3️⃣ LookAt
        Vector3 lookDir = (stick.position - transform.position);
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);
        }

        // 4️⃣ 玩家死亡检测
        Vector3 playerViewportPos = Camera.main.WorldToViewportPoint(player.position);
        if (playerViewportPos.y < 0f)
        {
            Debug.Log("Player Dead! Loading next scene...");
            LoadNextScene();
        }
    }

    void LoadNextScene()
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
