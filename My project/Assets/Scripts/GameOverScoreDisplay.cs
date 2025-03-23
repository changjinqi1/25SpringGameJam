using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        float lastScore = PlayerPrefs.GetFloat("LastScore", 0f); 
        scoreText.text = "Final Height: " + lastScore.ToString("F2") + "m";
    }
}
