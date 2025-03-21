using UnityEngine;

public class PlayerOrbitWithGravityAndCollision : MonoBehaviour
{
    public Transform stick;            // 柱子
    public float orbitSpeed = 2f;      // 旋转速度 (弧度/秒)
    public float radius = 2f;          // 围绕柱子半径
    private int direction = 1;         // 方向：1 = 顺时针, -1 = 逆时针

    private Rigidbody rb;
    private float currentAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 初始角度，根据起始位置
        Vector3 offset = transform.position - stick.position;
        currentAngle = Mathf.Atan2(offset.z, offset.x);
    }

    void Update()
    {
        // 空格键切换方向
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

        // 计算新的 XZ 位置
        Vector3 offset = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * radius;
        Vector3 targetXZ = stick.position + offset;

        // 保留 Rigidbody 的 Y 轴 (重力控制)
        Vector3 targetPosition = new Vector3(targetXZ.x, rb.position.y, targetXZ.z);

        // 移动刚体
        rb.MovePosition(targetPosition);

        // 朝向柱子中心 (可选)
        Vector3 lookDir = (stick.position - transform.position);
        lookDir.y = 0f; // 保证水平面朝向
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
            rb.MoveRotation(targetRot);
        }
    }
}