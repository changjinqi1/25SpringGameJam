using System.Collections;
using UnityEngine;

public class FragilePlatform : MonoBehaviour
{
    public Color warningColor = Color.red;    // Warning color
    public float warningTime = 0.5f;          // Time before falling

    private Color originalColor;
    private Renderer rend;
    private Rigidbody rb;

    private bool hasStartedFalling = false;   // Prevent multiple coroutines

    void Start()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;  // Prevent falling at start
            rb.useGravity = true;   // Ensure gravity is enabled
        }

        originalColor = rend.material.color;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !hasStartedFalling)
        {
            hasStartedFalling = true;
            StartCoroutine(FragileRoutine());
        }
    }

    IEnumerator FragileRoutine()
    {
        rend.material.color = warningColor;
        yield return new WaitForSeconds(warningTime);

        Debug.Log("Platform is falling!");

        if (rb != null)
        {
            rb.isKinematic = false;   // Enable physics
            Destroy(gameObject, 1f); // Destroys the platform after 1 seconds
        }
    }
}
