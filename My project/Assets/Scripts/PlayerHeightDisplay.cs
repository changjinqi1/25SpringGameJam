using UnityEngine;
using TMPro;

public class PlayerHeightDisplay : MonoBehaviour
{
    public Transform playerTransform; 
    public TextMeshProUGUI heightText; 

    void Update()
    {
        if (playerTransform != null && heightText != null)
        {
            float playerHeight = playerTransform.position.y;
            heightText.text = "Player Height: " + playerHeight.ToString("F2") + "m"; 
        }
    }
}
