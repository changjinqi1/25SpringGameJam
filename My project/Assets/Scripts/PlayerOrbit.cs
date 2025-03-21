using UnityEngine;

public class PlayerOrbitRigidbody : MonoBehaviour
{
    public Transform stick;            // The round stick to orbit around
    public float orbitSpeed = 5f;      // Speed of orbit movement
    public float radius = 2f;          // Distance from stick center
    private int direction = 1;         // 1 for clockwise, -1 for counterclockwise

    private Rigidbody rb;
    private float currentAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Initial angle based on player's position relative to stick
        Vector3 offset = transform.position - stick.position;
        currentAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        // Change direction on Space key press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction *= -1;
        }
    }

    void FixedUpdate()
    {
        if (stick == null || rb == null) return;

        // Update angle based on speed, direction, and time
        currentAngle += direction * orbitSpeed;

        // Keep angle within 0-360
        currentAngle %= 360f;

        // Calculate new position
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        Vector3 targetPos = stick.position + offset;

        // Move Rigidbody to target position
        rb.MovePosition(targetPos);

        // Optional: Face outward from the center
        rb.MoveRotation(Quaternion.LookRotation(Vector3.forward, offset.normalized));
    }
}
