using UnityEngine;

public class PlayerOrbit : MonoBehaviour
{
    public Transform stick;             // The central stick
    public float orbitSpeed = 2f;       // Orbit speed
    public float maxOrbitSpeed = 3f;    // Max orbit speed
    public float radius = 2f;           // Orbit radius

    public float fallSpeed = 2f;        // Falling speed
    public float maxFallSpeed = 5f;     // Max fall speed

    private int direction = 1;          // Orbit direction
    private Rigidbody rb;
    private float currentAngle = 0f;
    private bool collided = false;
    private bool isLocked = false;      // Lock orbit control externally
    private bool onStair = false;       // Is player currently on stair

    // External call to lock/unlock orbit (used when snapping to platform)
    public void LockOrbit(bool state)
    {
        isLocked = state;
    }

    // External call to reset orbit angle
    public void SetCurrentAngle(Vector3 playerPosition)
    {
        Vector3 offset = playerPosition - stick.position;
        currentAngle = Mathf.Atan2(offset.z, offset.x);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Initialize orbit angle
        Vector3 offset = transform.position - stick.position;
        currentAngle = Mathf.Atan2(offset.z, offset.x);
    }

    void Update()
    {
        // Press space to reverse direction
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction *= -1;
        }
    }

    void FixedUpdate()
    {
        if (stick == null || rb == null || isLocked) return;

        // 1️⃣ Handle Stair movement
        if (onStair)
        {
            // Apply continuous 45-degree upward movement
            Vector3 stairDirection = (transform.forward + Vector3.up).normalized * 5f; // Adjust speed here
            rb.velocity = stairDirection;
            return; // Skip orbit logic while on stair
        }

        // 2️⃣ Handle normal wall collision reversal
        if (collided)
        {
            direction *= -1;
            collided = false;
            return;
        }

        // 3️⃣ Normal orbit movement
        float currentSpeed = Mathf.Min(orbitSpeed, maxOrbitSpeed);
        currentAngle += direction * currentSpeed * Time.fixedDeltaTime;

        Vector3 offset = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * radius;
        Vector3 targetXZ = stick.position + offset;

        float yVelocity = Mathf.Clamp(rb.velocity.y - fallSpeed * Time.fixedDeltaTime, -maxFallSpeed, maxFallSpeed);

        Vector3 moveDirection = (targetXZ - rb.position);
        moveDirection.y = 0f;

        rb.velocity = new Vector3(moveDirection.x / Time.fixedDeltaTime, yVelocity, moveDirection.z / Time.fixedDeltaTime);

        // Face towards center
        Vector3 lookDir = (stick.position - transform.position);
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
            rb.MoveRotation(targetRot);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Wall side collision → reverse
        if (collision.collider.CompareTag("Wall"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                Vector3 normal = contact.normal;
                if (Mathf.Abs(normal.y) < 0.5f)
                {
                    Debug.Log("Side collision with Wall, reversing direction!");
                    collided = true;
                    break;
                }
            }
        }
        // Stair → Start stair movement
        else if (collision.collider.CompareTag("Stair"))
        {
            Debug.Log("Entered Stair");
            onStair = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Left stair
        if (collision.collider.CompareTag("Stair"))
        {
            Debug.Log("Exited Stair");
            onStair = false;
        }
    }
}
