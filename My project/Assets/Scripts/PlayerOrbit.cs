using UnityEngine;

public class PlayerOrbitRigidbody : MonoBehaviour
{
    public Transform stick;            // 柱子
    public float orbitSpeed = 90f;     // 旋转速度 (度/秒)
    public float radius = 2f;          // 距离柱子中心的半径
    private int direction = 1;         // 1 = 顺时针, -1 = 逆时针

    private Rigidbody rb;
    private float currentAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初始化角度，根据玩家当前位置
        Vector3 offset = transform.position - stick.position;
        currentAngle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        // 按下空格键改变方向
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction *= -1;
        }
    }

    void FixedUpdate()
    {
        if (stick == null || rb == null) return;

        // 更新角度
        currentAngle += direction * orbitSpeed * Time.fixedDeltaTime;
        currentAngle %= 360f;

        // 计算新的位置 (绕Y轴旋转)
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
        Vector3 targetPos = stick.position + offset;

        // 移动刚体到目标位置
        rb.MovePosition(targetPos);

        // 让玩家面朝柱子中心（可选）
        Vector3 lookDir = (stick.position - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
        rb.MoveRotation(targetRot);
    }
}
