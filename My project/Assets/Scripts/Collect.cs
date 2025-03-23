using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public Transform playerBase;
    public float yarnHeight = 1f;
    public float playerBodyHeight = 2f;

    // 添加音效变量
    public AudioSource collectSound; // 收集音效
    public AudioSource removeSound;  // 移除音效

    private float basePlayerY;
    private Rigidbody rb;
    private List<GameObject> collectedYarnBalls = new List<GameObject>();
    private BoxCollider detectCollider;

    private PlayerOrbit playerOrbit;
    private Collider currentPlatform = null;
    private bool isFalling = false;
    private Coroutine fallCheckCoroutine = null;

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
                // 从上撞平台 = 踩上平台
                float platformTopY = other.bounds.max.y;
                basePlayerY = platformTopY + 1.5f;
                currentPlatform = other;
                isFalling = false;

                Debug.Log("Stepped on new platform. BasePlayerY set to: " + basePlayerY);
            }
            else if (verticalFactor < -0.5f)
            {
                //从下撞平台 = 撞到头顶
                Debug.Log("Head bumped into ceiling!");

                if (collectedYarnBalls.Count > 0)
                {
                    RemoveYarnBall();
                    basePlayerY -= yarnHeight;
                    Debug.Log("Removed one yarn ball due to head bump.");
                }
                else
                {
                    Debug.Log("No yarn balls to remove on head bump.");
                }
            }
            else
            {
                // 其他方向碰撞处理，比如侧面撞墙
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
        if (isFalling) return;

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

        // 播放收集音效
        if (collectSound != null)
        {
            collectSound.Play();
        }

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

            // 播放移除音效
            if (removeSound != null)
            {
                removeSound.Play();
            }

            PositionYarnBalls();
            UpdateDetectCollider();
        }

        if (collectedYarnBalls.Count == 0)
        {
            GetComponentInChildren<PlayerAnimationController>()?.OnYarnListEmpty();
        }
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
        Debug.Log("Hit Wall: " + wallCollider.gameObject.name);

        int totalBalls = collectedYarnBalls.Count;
        if (totalBalls > 0)
        {
            Debug.Log("Removing ALL " + totalBalls + " yarn balls!");
            for (int i = 0; i < totalBalls; i++)
            {
                RemoveYarnBall();
            }
        }

        Transform teleportPoint = wallCollider.transform.Find("TeleportPoint");

        if (teleportPoint != null)
        {
            Vector3 targetPos = teleportPoint.position;

            Collider playerCollider = GetComponent<Collider>();
            if (playerCollider != null) playerCollider.enabled = false;

            if (playerOrbit != null) playerOrbit.LockOrbit(true);

            rb.position = targetPos;
            Debug.Log("Moved player to TeleportPoint at: " + targetPos);

            if (playerOrbit != null) playerOrbit.SetCurrentAngle(rb.position);

            basePlayerY = targetPos.y;

            StartCoroutine(ReenableOrbit(0.1f));
        }
        else
        {
            Debug.LogWarning("TeleportPoint child not found on wall: " + wallCollider.name);
        }
    }

    IEnumerator ReenableOrbit(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerOrbit != null) playerOrbit.LockOrbit(false);

        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null) playerCollider.enabled = true;
    }
}
