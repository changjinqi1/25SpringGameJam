using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBackgroundLooper : MonoBehaviour

{
    public Transform[] backgrounds; // Assign 2 background transforms
    public float scrollSpeed = 1f; // Units per second
    public Camera cam;

    private float backgroundHeight;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        // Get the world height of the camera view
        float height = 2f * cam.orthographicSize;
        backgroundHeight = height;

        // Resize each background to fill the camera vertically
        foreach (Transform bg in backgrounds)
        {
            SpriteRenderer sr = bg.GetComponent<SpriteRenderer>();
            if (sr)
            {
                float width = height * cam.aspect;
                bg.localScale = new Vector3(width / sr.bounds.size.x, height / sr.bounds.size.y, 1f);
            }
        }
    }

    void Update()
    {
        foreach (Transform bg in backgrounds)
        {
            bg.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

            // If this background is completely below the camera view, move it above the other
            if (bg.position.y < -backgroundHeight)
            {
                float highestY = GetHighestBackgroundY();
                bg.position = new Vector3(bg.position.x, highestY + backgroundHeight, bg.position.z);
            }
        }
    }

    float GetHighestBackgroundY()
    {
        float highestY = float.MinValue;
        foreach (Transform bg in backgrounds)
        {
            if (bg.position.y > highestY)
                highestY = bg.position.y;
        }
        return highestY;
    }
}

