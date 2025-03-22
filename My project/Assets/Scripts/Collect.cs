using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public Transform playerBase; // The base point under the player's feet
    public float yarnHeight = 1f; // The height of each ball of yarn
    private float basePlayerY;
    private Rigidbody rb;


    void Start()
    {
        basePlayerY = transform.position.y;
        rb = GetComponent<Rigidbody>();
    }


    private List<GameObject> collectedYarnBalls = new List<GameObject>(); // yarn balls list



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YarnBall")) // tag of yarn balls
        {
            // Increase player height
            collectedYarnBalls.Add(other.gameObject);
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;


            // Set the yarn ball as a child object of playerBase (very important)
            other.transform.SetParent(playerBase);



            // Move the ball of yarn to the player's feet
            PositionYarnBalls();



            Debug.Log("Collected Yarn Ball! Total: " + collectedYarnBalls.Count);
        }
    }

    //void UpdatePlayerHeight()
    //{
    // Adjust the player's height based on the number of balls of yarn

    //   float newY = collectedYarnBalls.Count * yarnHeight;
    // transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    //}



    // Player Position update
    void FixedUpdate()
    {
        float offsetY = collectedYarnBalls.Count * yarnHeight;
        Vector3 targetPosition = new Vector3(rb.position.x, basePlayerY + offsetY, rb.position.z);
        rb.MovePosition(targetPosition);
    }



    void PositionYarnBalls()
    {
        for (int i = 0; i < collectedYarnBalls.Count; i++)
        {
            GameObject yarnBall = collectedYarnBalls[i];
            Vector3 localOffset = new Vector3(0, -((i + 1) * yarnHeight), 0); // ÏòÏÂµþ
            yarnBall.transform.localPosition = playerBase.localPosition + localOffset;
        }
    }


    // Removing Yarn Balls
    public void RemoveYarnBall()
    {
        if (collectedYarnBalls.Count > 0)
        {
            GameObject removed = collectedYarnBalls[0];
            collectedYarnBalls.RemoveAt(0);
            Destroy(removed);

            PositionYarnBalls(); 
        }
    }

}
