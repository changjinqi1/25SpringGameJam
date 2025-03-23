using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTcollect : MonoBehaviour
{
    public Transform playerBase;
    public float yarnHeight = 1f;
    public float playerBodyHeight = 2f;
    public float teleportDuration = 0.5f;

    public ParticleSystem clearEffect; // ✅ 粒子特效

    private float basePlayerY;
    private Rigidbody rb;
    private List<GameObject> collectedYarnBalls = new List<GameObject>();
    private BoxCollider detectCollider;
    private PlayerOrbit playerOrbit;
    private Collider currentPlatform = null;
    private bool isFalling = false;

    void Start()
    {
        basePlayerY = transform.position.y;
        rb = GetComponent<Rigidbody>();

        detectCollider = transform.Find("YarnStackTrigger").GetComponent<BoxCollider>();
        if (detectCollider != null)
        {
            detectCollider.isTrigger = true;
            UpdateDetectCollider();
        }
        else
        {
            Debug.LogError("YarnStackTrigger (BoxCollider) not found!");
        }

        playerOrbit = GetComponent<PlayerOrbit>();
        if (playerOrbit == null)
        {
            Debug.LogError("PlayerOrbit script not found!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YarnBall"))
        {
            CollectYarnBall(other.gameObject);
            GetComponentInChildren<PlayerAnimationController>()?.OnYarnCollected();
        }

        if (other.CompareTag("Wall"))
        {
            Vector3 direction = transform.position - other.ClosestPoint(transform.position);
            float verticalFactor = Vector3.Dot(direction.normalized, Vector3.up);

            if (verticalFactor > 0.5f)
            {
                // 脚下踩平台
                float platformTopY = other.bounds.max.y;
                basePlayerY = platformTopY + 1.5f;
                currentPlatform = other;
                isFalling = false;

                Debug.Log("Stepped on new platform. BasePlayerY set to: " + basePlayerY);
            }
            else if (verticalFactor < -0.5f)
            {
                // 撞头
                Debug.Log("Head bumped into ceiling!");

                if (collectedYarnBalls.Count > 0)
                {
                    RemoveYarnBall();
                    basePlayerY -= yarnHeight;
                    Debug.Log("Removed one yarn ball due to head bump.");
                }
            }
            else
            {
                // 侧面撞墙
                CheckWallHeightAndRemoveBalls(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall") && other == currentPlatform)
        {
            currentPlatform = null;
            isFalling = true;
            Debug.Log("Left platform. Falling...");
        }
    }

    void FixedUpdate()
    {
        if (isFalling)
        {
            return;
        }

        // -----------------------------
        // 👣 Raycast 检测脚下是否仍有平台
        // -----------------------------
        float raycastLength = playerBodyHeight + collectedYarnBalls.Count * 1f + 0.3f;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // 稍微抬高一点防止贴地问题

        RaycastHit hit;
        if (!Physics.Raycast(rayOrigin, Vector3.down, out hit, raycastLength))
        {
            isFalling = true;
            currentPlatform = null;
            Debug.DrawRay(rayOrigin, Vector3.down * raycastLength, Color.red);
            Debug.Log("Lost ground support. Player is falling.");
            return;
        }
        else
        {
            Debug.DrawRay(rayOrigin, Vector3.down * raycastLength, Color.green);
        }

        // -----------------------------
        // 🧶 正常位置锁定在平台顶部 + 毛线球高度
        // -----------------------------
        if (collectedYarnBalls.Count > 0)
        {
            float offsetY = collectedYarnBalls.Count * yarnHeight;
            Vector3 targetPosition = new Vector3(rb.position.x, basePlayerY + offsetY, rb.position.z);
            rb.MovePosition(targetPosition);
        }
    }


    void CollectYarnBall(GameObject yarnBall)
    {
        collectedYarnBalls.Add(yarnBall);

        Rigidbody yarnRb = yarnBall.GetComponent<Rigidbody>();
        if (yarnRb != null) yarnRb.isKinematic = true;

        Collider playerCollider = GetComponent<Collider>();
        Collider yarnCollider = yarnBall.GetComponent<Collider>();
        if (playerCollider != null && yarnCollider != null)
        {
            Physics.IgnoreCollision(playerCollider, yarnCollider);
            yarnCollider.enabled = false;
        }

        yarnBall.transform.SetParent(playerBase);
        yarnBall.transform.localPosition = Vector3.zero;

        PositionYarnBalls();
        UpdateDetectCollider();

        Debug.Log("Collected Yarn Ball! Total: " + collectedYarnBalls.Count);
    }

    void PositionYarnBalls()
    {
        for (int i = 0; i < collectedYarnBalls.Count; i++)
        {
            GameObject yarnBall = collectedYarnBalls[i];
            Vector3 localOffset = new Vector3(0, -(i + 1) * yarnHeight, 0);
            yarnBall.transform.localPosition = localOffset;
        }
    }

    public void RemoveYarnBall()
    {
        if (collectedYarnBalls.Count > 0)
        {
            GameObject removed = collectedYarnBalls[collectedYarnBalls.Count - 1];
            collectedYarnBalls.RemoveAt(collectedYarnBalls.Count - 1);
            Destroy(removed);

            PositionYarnBalls();
            UpdateDetectCollider();
        }

        if (collectedYarnBalls.Count == 0)
        {
            GetComponentInChildren<PlayerAnimationController>()?.OnYarnListEmpty();
        }
    }
    public bool HasYarnBalls()
    {
        return collectedYarnBalls.Count > 0;
    }

  

    void UpdateDetectCollider()
    {
        if (detectCollider != null)
        {
            float totalHeight = collectedYarnBalls.Count * yarnHeight;
            float colliderHeight = Mathf.Max(yarnHeight, totalHeight);
            detectCollider.size = new Vector3(1.5f, colliderHeight, 1.5f);
            detectCollider.center = new Vector3(0, -colliderHeight / 2f, 0);
        }
    }

    void CheckWallHeightAndRemoveBalls(Collider wallCollider)
    {
        Debug.Log("Side hit Wall: " + wallCollider.gameObject.name);

        int totalBalls = collectedYarnBalls.Count;
        if (totalBalls > 0)
        {
            Debug.Log("Removing ALL " + totalBalls + " yarn balls!");
            for (int i = 0; i < totalBalls; i++)
            {
                RemoveYarnBall();
            }

            // ✅ 播放粒子特效
            if (clearEffect != null)
            {
                clearEffect.Play();
            }
        }

        Transform teleportPoint = wallCollider.transform.Find("TeleportPoint");

        if (teleportPoint != null)
        {
            Vector3 targetPos = teleportPoint.position;
            StartCoroutine(SmoothTeleport(targetPos));
        }
        else
        {
            Debug.LogWarning("TeleportPoint child not found on wall: " + wallCollider.name);
        }
    }

    IEnumerator SmoothTeleport(Vector3 targetPos)
    {
        Collider playerCollider = GetComponent<Collider>();

        // Disable collider + lock orbit
        if (playerCollider != null) playerCollider.enabled = false;
        if (playerOrbit != null) playerOrbit.LockOrbit(true);

        Vector3 startPos = rb.position;
        float elapsed = 0f;

        while (elapsed < teleportDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / teleportDuration;

            Vector3 newPos = Vector3.Lerp(startPos, targetPos, t);
            rb.MovePosition(newPos);

            yield return null;
        }

        rb.MovePosition(targetPos);

        if (playerOrbit != null)
        {
            playerOrbit.SetCurrentAngle(targetPos);
        }

        basePlayerY = targetPos.y;
        isFalling = false;
        currentPlatform = null;

        StartCoroutine(ReenableOrbit(0.1f));
    }

    IEnumerator ReenableOrbit(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerOrbit != null) playerOrbit.LockOrbit(false);

        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null) playerCollider.enabled = true;
    }
#if UNITY_EDITOR
void OnDrawGizmos()
{
    if (!Application.isPlaying) return;

    float raycastLength = playerBodyHeight + collectedYarnBalls.Count * 1f + 0.3f;
    Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;

    Gizmos.color = Color.yellow;
    Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * raycastLength);
}
#endif

    //int animator condition
    public int GetYarnBallCount()
    {
        return collectedYarnBalls.Count;
    }
}
