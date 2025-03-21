using UnityEngine;
using TMPro;

public class PlayerHeightDisplay : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's Transform component to get height
    public TextMeshProUGUI heightText; // Reference to the TextMeshProUGUI component for displaying height

    void Update()
    {
        if (playerTransform != null && heightText != null)
        {
            // Get the player's height (Y position)
            float playerHeight = playerTransform.position.y;

            // Update the text to display the height with two decimal places and "m" unit
            heightText.text = "Player Height: " + playerHeight.ToString("F2") + "m";
        }
    }
}
