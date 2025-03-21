using UnityEngine;

public class PlayerOrbitRB : MonoBehaviour
{
    public Transform stick;             // Center object
    public float radius = 2f;           // Distance from stick
    public float orbitSpeed = 100f;     // Degrees per second
    private float currentAngle = 0f;    // Current angle around stick
    private int direction = 1;          // 1 = clockwise, -1 = counterclockwise
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calculate initial angle based on position
        Vector2 dir = transform.position - stick.position;
        currentAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    void Update()
    {
        // Flip direction on Space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction *= -1;
        }
    }

    void FixedUpdate()
    {
        if (stick == null) return;

        // Update angle
        currentAngle += direction * orbitSpeed * Time.fixedDeltaTime;
        currentAngle = currentAngle % 360f;

        // Calculate new position
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 newPos = new Vector2(
            stick.position.x + Mathf.Cos(rad) * radius,
            stick.position.y + Mathf.Sin(rad) * radius
        );

        // Move Rigidbody
        rb.MovePosition(newPos);
    }
}
