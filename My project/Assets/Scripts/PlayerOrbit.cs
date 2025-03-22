using UnityEngine;

public class PlayerOrbitWithGravityAndCollision : MonoBehaviour
{
    public Transform stick;             // 柱子
    public float orbitSpeed = 2f;       // 旋转速度 (弧度/秒)
    public float maxOrbitSpeed = 3f;    // 最大旋转速度
    public float radius = 2f;           // 围绕柱子的半径
    public LayerMask obstacleLayers;    // 可检测的障碍物层
    public float obstacleRayDistance = 0.6f; // 检测距离

    private int direction = 1;          // 1 = 顺时针，-1 = 逆时针
    private Rigidbody rb;
    private float currentAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 计算初始角度
        Vector3 offset = transform.position - stick.position;
        currentAngle = Mathf.Atan2(offset.z, offset.x);
    }

    void Update()
    {
        // 按空格切换方向
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction *= -1;
        }
    }

    void FixedUpdate()
    {
        if (stick == null || rb == null) return;

        // 检测前方是否有障碍物
        if (IsObstacleAhead())
        {
            direction *= -1; // 碰到障碍反转方向
            return;          // 本帧不移动
        }

        // 限制最大速度
        orbitSpeed = Mathf.Min(orbitSpeed, maxOrbitSpeed);

        // 更新角度
        currentAngle += direction * orbitSpeed * Time.fixedDeltaTime;

        // 计算新的位置（XZ平面）
        Vector3 offset = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * radius;
        Vector3 targetXZ = stick.position + offset;
        Vector3 targetPosition = new Vector3(targetXZ.x, rb.position.y, targetXZ.z);

        // 移动刚体
        rb.MovePosition(targetPosition);

        // 朝向柱子中心
        Vector3 lookDir = (stick.position - transform.position);
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
            rb.MoveRotation(targetRot);
        }
    }

    // 发射射线检测障碍物
    bool IsObstacleAhead()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 moveDir = Quaternion.Euler(0, direction * 10f, 0) * transform.forward;

        // 调试用射线（Scene视图可见）
        Debug.DrawRay(origin, moveDir * obstacleRayDistance, Color.red);

        return Physics.Raycast(origin, moveDir, obstacleRayDistance, obstacleLayers);
    }
}
