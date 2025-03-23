using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;

            // Get the StickPoint of the current level
            Transform stickPoint = transform.root.Find("StickPoint");

            // Let the player bind the new column
            other.GetComponent<PlayerOrbit>().stick = stickPoint;

            // Tell the LevelGenerator to generate the next level
            FindObjectOfType<LevelGenerator>().SpawnNextLevel();
        }
    }
}
