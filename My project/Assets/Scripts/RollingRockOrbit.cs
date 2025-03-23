using UnityEngine;

public class RollingRockOrbitFixed : MonoBehaviour
{
    public Transform stick;             // The central pillar
    public float orbitSpeed = 3f;       // Speed of orbit (tangential)
    public float radius = 3f;           // Fixed radius
    public float maxSpeed = 6f;         // Limit speed
    public float yDrag = 1f;            // Control fall smoothness

    private Rigidbody rb;
    private float currentAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        // Freeze rotation on XZ axes, let Y rotate naturally (simulate rolling)
    }

    void FixedUpdate()
    {
        if (stick == null || rb == null) return;

        // 1️⃣ 强制控制 XZ 位置 — 永远保持 radius
        currentAngle += orbitSpeed * Time.fixedDeltaTime;

        Vector3 offset = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * radius;
        Vector3 targetPosXZ = stick.position + offset;

        Vector3 currentPos = rb.position;

        // Y轴保持当前物理效果
        Vector3 finalPos = new Vector3(targetPosXZ.x, currentPos.y, targetPosXZ.z);

        // 直接设置位置
        rb.MovePosition(finalPos);

        // 2️⃣ 给一个持续向前的速度 → 模拟滚动
        Vector3 tangentDir = Vector3.Cross(Vector3.up, offset.normalized);
        rb.velocity = new Vector3(tangentDir.x * orbitSpeed, rb.velocity.y * (1 - yDrag * Time.fixedDeltaTime), tangentDir.z * orbitSpeed);

        // 3️⃣ 保持面朝柱子中心
        Vector3 lookDir = (stick.position - transform.position);
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
            rb.MoveRotation(targetRot);
        }
    }
}
