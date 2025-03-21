using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Collect : MonoBehaviour
{
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

            Debug.Log("Collected Yarn Ball! Total: " + collectedYarnBalls.Count);
        }
    }

    void UpdatePlayerHeight()
    {
        // ����ë��������������Ҹ߶�
        float newY = collectedYarnBalls.Count * yarnHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void PositionYarnBalls()
    {
        // ��������ë�����γ�һ����è��ߵ�Ч��
        for (int i = 0; i < collectedYarnBalls.Count; i++)
        {
            Vector3 newPos = playerBase.position + new Vector3(0, i * yarnHeight, 0);
            collectedYarnBalls[i].transform.position = newPos;
        }
    }

    // �Ƴ�ë����
    public void RemoveYarnBall()
    {
        if (collectedYarnBalls.Count > 0)
        {
            //�������濪ʼ�Ƴ�
            GameObject removedBall = collectedYarnBalls[0];
            collectedYarnBalls.RemoveAt(0);
            Destroy(removedBall);

            UpdatePlayerHeight();
            PositionYarnBalls();
        }
    }

}
