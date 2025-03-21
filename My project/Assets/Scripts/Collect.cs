using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    public Transform playerBase; // The base point under the player's feet
    public float yarnHeight = 1f; // The height of each ball of yarn
    private List<GameObject> collectedYarnBalls = new List<GameObject>(); // yarn balls list



    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YarnBall")) // tag of yarn balls
        {
            // Increase player height
            collectedYarnBalls.Add(other.gameObject);
            UpdatePlayerHeight();

            // Move the ball of yarn to the player's feet
            PositionYarnBalls();

            // Turn off the physics of the ball of yarn so it follows the player
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true; // Make it unaffected by gravity
            other.transform.SetParent(transform); // Make the ball of yarn follow the player


            Debug.Log("Collected Yarn Ball! Total: " + collectedYarnBalls.Count);
        }
    }

    void UpdatePlayerHeight()
    {

        // ����ë��������������Ҹ߶�

        // Adjust the player's height based on the number of balls of yarn

        float newY = collectedYarnBalls.Count * yarnHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void PositionYarnBalls()
    {

        // ��������ë�����γ�һ����è��ߵ�Ч��

        // Arrange the balls of yarn in order to create an effect of raising the cat.

        for (int i = 0; i < collectedYarnBalls.Count; i++)
        {
            Vector3 newPos = playerBase.position + new Vector3(0, i * yarnHeight, 0);
            collectedYarnBalls[i].transform.position = newPos;
        }
    }


    // Removing Yarn Balls
    public void RemoveYarnBall()
    {
        if (collectedYarnBalls.Count > 0)
        {

            //
            //Remove from the bottom
            GameObject removedBall = collectedYarnBalls[0];
            collectedYarnBalls.RemoveAt(0);
            Destroy(removedBall);

            UpdatePlayerHeight();
            PositionYarnBalls();
        }
    }

}
