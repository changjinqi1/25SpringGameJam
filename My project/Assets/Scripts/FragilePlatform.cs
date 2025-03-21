using System.Collections;
using UnityEngine;

public class FragilePlatform : MonoBehaviour
{
    public Color warningColor = Color.red;  // Color when warning
    public float warningTime = 1f;          // Time before falling
    private Color originalColor;

    private Renderer rend;
    private Rigidbody rb;

    private bool playerOnPlatform = false;  // Track if player is still on

    void Start()
    {
        rend = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
            rb.isKinematic = true; // Make sure it doesn't fall at start

        originalColor = rend.material.color;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !playerOnPlatform)
        {
            playerOnPlatform = true;
            StartCoroutine(FragileRoutine());
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerOnPlatform = false;
        }
    }

    IEnumerator FragileRoutine()
    {
        // Change color to red
        rend.material.color = warningColor;

        // Wait 1 second
        yield return new WaitForSeconds(warningTime);

        // Check if player is still on
        if (playerOnPlatform)
        {
            Debug.Log("Platform breaks!");
            if (rb != null)
                rb.isKinematic = false; // Enable physics, let it fall
        }
        else
        {
            // No player, revert color
            rend.material.color = originalColor;
        }
    }
}
