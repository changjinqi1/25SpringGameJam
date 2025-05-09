using UnityEngine;
using System.Collections;

public class IcePlatform : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    public float speedMultiplier = 1.5f; // Multiplier for orbitSpeed
    public float boostDuration = 2f;     // Duration of the boost in seconds

    private Coroutine boostCoroutine;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerOrbit orbitScript = collision.gameObject.GetComponent<PlayerOrbit>();

            if (orbitScript != null)
            {
                if (boostCoroutine != null)
                    StopCoroutine(boostCoroutine);

                boostCoroutine = StartCoroutine(SpeedBoost(orbitScript));
            }
        }
    }

    private IEnumerator SpeedBoost(PlayerOrbit playerOrbit)
    {
        float originalSpeed = playerOrbit.orbitSpeed;

        playerOrbit.orbitSpeed *= speedMultiplier;

        yield return new WaitForSeconds(boostDuration);

        playerOrbit.orbitSpeed = originalSpeed;
    }
}
