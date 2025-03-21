using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Collect : MonoBehaviour
{
<<<<<<< HEAD
    public Transform playerBase; // ��ҽ��µĻ�����
    public float yarnHeight = 1f; // ÿ��ë����ĸ߶�
    private List<GameObject> collectedYarnBalls = new List<GameObject>(); // �洢���ռ���ë����

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YarnBall")) // ë���򣡣�������������Tag����������������������������������������
        {
            // ������Ҹ߶�
            collectedYarnBalls.Add(other.gameObject);
            UpdatePlayerHeight();

            // ��ë�����ƶ�����ҽ���
            PositionYarnBalls();

            // �ر�ë���������Ӱ�죬�����������
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true; // ������������Ӱ��
            other.transform.SetParent(transform); // ��ë�����������ƶ�
=======
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
>>>>>>> 3edfeab975bd9214be53ce954970333e36e7671f

            Debug.Log("Collected Yarn Ball! Total: " + collectedYarnBalls.Count);
        }
    }

    void UpdatePlayerHeight()
    {
<<<<<<< HEAD
        // ����ë��������������Ҹ߶�
=======
        // Adjust the player's height based on the number of balls of yarn
>>>>>>> 3edfeab975bd9214be53ce954970333e36e7671f
        float newY = collectedYarnBalls.Count * yarnHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void PositionYarnBalls()
    {
<<<<<<< HEAD
        // ��������ë�����γ�һ����è��ߵ�Ч��
=======
        // Arrange the balls of yarn in order to create an effect of raising the cat.
>>>>>>> 3edfeab975bd9214be53ce954970333e36e7671f
        for (int i = 0; i < collectedYarnBalls.Count; i++)
        {
            Vector3 newPos = playerBase.position + new Vector3(0, i * yarnHeight, 0);
            collectedYarnBalls[i].transform.position = newPos;
        }
    }

<<<<<<< HEAD
    // �Ƴ�ë����
=======
    // Removing Yarn Balls
>>>>>>> 3edfeab975bd9214be53ce954970333e36e7671f
    public void RemoveYarnBall()
    {
        if (collectedYarnBalls.Count > 0)
        {
<<<<<<< HEAD
            //�������濪ʼ�Ƴ�
=======
            //Remove from the bottom
>>>>>>> 3edfeab975bd9214be53ce954970333e36e7671f
            GameObject removedBall = collectedYarnBalls[0];
            collectedYarnBalls.RemoveAt(0);
            Destroy(removedBall);

            UpdatePlayerHeight();
            PositionYarnBalls();
        }
    }

}
