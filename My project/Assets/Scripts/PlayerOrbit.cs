using UnityEngine;

public class PlayerOrbit : MonoBehaviour
{
    public Transform stick;          // The round stick (center to orbit around)
    public float orbitSpeed = 100f;  // Speed of rotation
    private int direction = 1;       // 1 for clockwise, -1 for counterclockwise

    void Update()
    {
        if (stick == null) return;

        // Rotate around the stick
        transform.RotateAround(stick.position, Vector3.forward, direction * orbitSpeed * Time.deltaTime);

        // Flip direction when pressing Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            direction *= -1;
        }
    }
}
