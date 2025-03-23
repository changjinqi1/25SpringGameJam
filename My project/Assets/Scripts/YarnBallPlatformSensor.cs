using UnityEngine;

public class YarnBallPlatformSensor : MonoBehaviour
{//把这个脚本的功能改成检测地面
    [Header("平台碰撞检测设置")]
    public LayerMask platformLayers;
    public float groundCheckDistance = 0.2f;
    public float groundOffset = 0.05f;

    public Collect collectScript; // 在游戏场景里找带有这个script的物体，也就是玩家。
    private bool isTouchingPlatform = false;
    // YarnBallPlatformSensor.cs

    void Awake()
    {
        if (collectScript == null)
        {
            collectScript = FindClosestCollector();
        }
    }

    private Collect FindClosestCollector()
    {
        Collect[] all = FindObjectsOfType<Collect>();
        Collect closest = null;
        float minDist = Mathf.Infinity;

        foreach (var col in all)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col;
            }
        }

        return closest;
    }

    void FixedUpdate()
    {
        CheckGroundContact();
    }

    private void CheckGroundContact()
    {
        Vector3 origin = transform.position + Vector3.up * groundOffset;
        isTouchingPlatform = Physics.Raycast(origin, Vector3.down, groundCheckDistance + groundOffset, platformLayers);
    }

    public bool IsTouchingPlatform()
    {
        return isTouchingPlatform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up * groundOffset, Vector3.down * (groundCheckDistance + groundOffset));
    }
}
