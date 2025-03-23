using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public Transform playerBase;
    public float yarnHeight = 1f;
    public float playerBodyHeight = 2f;

    private float basePlayerY;
    private Rigidbody rb;
    private List<GameObject> collectedYarnBalls = new List<GameObject>();
    private BoxCollider detectCollider;

    private PlayerOrbit playerOrbit; // 🔑 新增

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

        // 🔑 获取 PlayerOrbit 脚本
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
        }
        else if (other.CompareTag("Wall"))
        {
            CheckWallHeightAndRemoveBalls(other);
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

    void FixedUpdate()
    {
        if (collectedYarnBalls.Count > 0)
        {
            float offsetY = collectedYarnBalls.Count * yarnHeight;
            Vector3 targetPosition = new Vector3(rb.position.x, basePlayerY + offsetY, rb.position.z);
            rb.MovePosition(targetPosition);
        }
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

        // Remove all yarn balls
        int totalBalls = collectedYarnBalls.Count;
        if (totalBalls > 0)
        {
            Debug.Log("Removing ALL " + totalBalls + " yarn balls!");
            for (int i = 0; i < totalBalls; i++)
            {
                RemoveYarnBall();
            }
        }

        // 🔥 Find child TeleportPoint
        Transform teleportPoint = wallCollider.transform.Find("TeleportPoint");

        if (teleportPoint != null)
        {
            Vector3 targetPos = teleportPoint.position;

            // Disable collider temporarily
            Collider playerCollider = GetComponent<Collider>();
            if (playerCollider != null) playerCollider.enabled = false;

            // Lock orbit
            if (playerOrbit != null)
            {
                playerOrbit.LockOrbit(true);
            }

            // Move player to TeleportPoint position
            rb.position = targetPos;
            Debug.Log("Moved player to TeleportPoint at: " + targetPos);

            // Update orbit angle
            if (playerOrbit != null)
            {
                playerOrbit.SetCurrentAngle(rb.position);
            }

            // Optional: Adjust basePlayerY if needed (depending on how FixedUpdate adjusts Y)
            basePlayerY = targetPos.y;

            // Delay re-enable collider and orbit
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

        if (playerOrbit != null)
        {
            playerOrbit.LockOrbit(false);
        }

        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }
    }


}
