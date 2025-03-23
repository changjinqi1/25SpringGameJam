using UnityEngine;

public class CameraRotationLock : MonoBehaviour
{
    public Vector3 lockedRotationEuler = new Vector3(0f, 0f, 0f); // 固定角度 (欧拉角)

    void LateUpdate()
    {
        // 始终把相机旋转锁到固定角度
        transform.rotation = Quaternion.Euler(lockedRotationEuler);
    }
}
