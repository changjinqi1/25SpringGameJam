using UnityEngine;
using System.Collections;

public class IcePlatform : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    public float speedMultiplier = 1.5f; // Multiplier for speed increase
    public float boostDuration = 2f; // Duration of the speed boost in seconds

    [Header("Player Settings")]
    public Rigidbody2D playerRigidbody; // Reference to the player's Rigidbody2D
    public float baseSpeed = 5f; // Player's normal speed

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player stepped on the platform
        if (collision.gameObject.CompareTag("Player") && playerRigidbody != null)
        {
            // Stop any existing boost coroutine before starting a new one
            StopAllCoroutines();
            StartCoroutine(SpeedBoost());
        }
    }

    private IEnumerator SpeedBoost()
    {
        float boostedSpeed = baseSpeed * speedMultiplier; // Calculate new speed
        playerRigidbody.velocity = new Vector2(boostedSpeed * Mathf.Sign(playerRigidbody.velocity.x), playerRigidbody.velocity.y);

        yield return new WaitForSeconds(boostDuration); // Wait for the boost duration

        // Reset speed after boost time ends
        playerRigidbody.velocity = new Vector2(baseSpeed * Mathf.Sign(playerRigidbody.velocity.x), playerRigidbody.velocity.y);
    }
}
