using UnityEngine;

public class CameraFollowUpOnly : MonoBehaviour
{
    public Transform player;         // 玩家
    public float smoothSpeed = 2f;   // 跟随平滑度

    private float minY;              // 相机当前 Y 最低位置

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned!");
        }
        minY = transform.position.y; // 初始相机 Y 位置
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 camPos = transform.position;

        // 只考虑 Y 轴
        if (player.position.y > camPos.y)
        {
            camPos.y = Mathf.Lerp(camPos.y, player.position.y, smoothSpeed * Time.deltaTime);
            minY = camPos.y; // 更新相机最低点
        }

        transform.position = camPos;

        // 检测玩家是否掉出屏幕底部
        Vector3 playerViewportPos = Camera.main.WorldToViewportPoint(player.position);

        if (playerViewportPos.y < 0f)
        {
            Debug.Log("Player Dead! (Out of screen)");
        }
    }
}
