using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundScroll : MonoBehaviour
{
    public RectTransform[] layers; 
    public float[] speeds; 
    public float resetPosition = 1200f; 

    private Vector3[] startPositions;
    private int[] directions; 

    void Start()
    {
        if (layers.Length != speeds.Length)
        {
            Debug.LogError("ParallaxBackgroundScroll:");
            return;
        }

        startPositions = new Vector3[layers.Length];
        directions = new int[layers.Length]; 

        for (int i = 0; i < layers.Length; i++)
        {
            startPositions[i] = layers[i].anchoredPosition;
            directions[i] = -1; 
        }
    }

    void Update()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].anchoredPosition += Vector2.right * speeds[i] * directions[i] * Time.deltaTime;

            if (layers[i].anchoredPosition.x <= -resetPosition)
            {
                directions[i] = 1;
            }
            else if (layers[i].anchoredPosition.x >= resetPosition)
            {
                directions[i] = -1;
            }
        }
    }
}
