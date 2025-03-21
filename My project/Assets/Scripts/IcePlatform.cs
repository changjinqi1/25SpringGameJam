using UnityEngine;
using System.Collections;

public class IcePlatform : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    public float speedMultiplier = 1.5f; // Multiplier for speed increase
    public float boostDuration = 2f; // Duration of the speed boost in seconds

    [Header("Player Settings")]
    public float baseSpeed = 5f; // Player's normal speed

    private Coroutine boostCoroutine;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player stepped on the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                // Stop any existing coroutine running on this script
                if (boostCoroutine != null)
                {
                    StopCoroutine(boostCoroutine);
                }

                boostCoroutine = StartCoroutine(SpeedBoost(playerRb));
            }
        }
    }

    private IEnumerator SpeedBoost(Rigidbody2D playerRb)
    {
        float boostedSpeed = baseSpeed * speedMultiplier;

        // Apply boosted speed
        playerRb.velocity = new Vector2(boostedSpeed * Mathf.Sign(playerRb.velocity.x), playerRb.velocity.y);

        yield return new WaitForSeconds(boostDuration);

        // Reset to base speed
        playerRb.velocity = new Vector2(baseSpeed * Mathf.Sign(playerRb.velocity.x), playerRb.velocity.y);
    }
}
