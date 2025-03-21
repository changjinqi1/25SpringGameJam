using UnityEngine;
using TMPro;  

public class ScoreDisplay : MonoBehaviour
{
    public Transform catTransform; 
    public TMP_Text scoreText; 

    void Update()
    {
        if (catTransform != null && scoreText != null)
        {
            int score = Mathf.FloorToInt(catTransform.position.y); // È¡Õû
            scoreText.text = score.ToString(); 
        }
    }
}
