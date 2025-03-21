using UnityEngine;

public class VerticalPingPongMovement : MonoBehaviour
{
    public float moveRange = 2f;       // Total up-down distance
    public float moveSpeed = 2f;       // Speed of movement
    private float startY;              // Initial Y position

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        // Calculate new Y position using Mathf.PingPong
        float newY = startY + Mathf.PingPong(Time.time * moveSpeed, moveRange) - (moveRange / 2f);

        // Apply new position
        Vector3 newPosition = new Vector3(transform.position.x, newY, transform.position.z);
        transform.position = newPosition;
    }
}
