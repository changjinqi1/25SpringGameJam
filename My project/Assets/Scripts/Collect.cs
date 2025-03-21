using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Collect : MonoBehaviour
{
    public Transform playerBase; // 玩家脚下的基础点
    public float yarnHeight = 1f; // 每个毛线球的高度
    private List<GameObject> collectedYarnBalls = new List<GameObject>(); // 存储已收集的毛线球

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("YarnBall")) // 毛线球！！！！！！！打Tag！！！！！！！！！！！！！！！！！！！！
        {
            // 增加玩家高度
            collectedYarnBalls.Add(other.gameObject);
            UpdatePlayerHeight();

            // 让毛线球移动到玩家脚下
            PositionYarnBalls();

            // 关闭毛线球的物理影响，让它跟随玩家
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true; // 让它不受重力影响
            other.transform.SetParent(transform); // 让毛线球跟随玩家移动

            Debug.Log("Collected Yarn Ball! Total: " + collectedYarnBalls.Count);
        }
    }

    void UpdatePlayerHeight()
    {
        // 根据毛线球数量调整玩家高度
        float newY = collectedYarnBalls.Count * yarnHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void PositionYarnBalls()
    {
        // 依次排列毛线球，形成一个把猫垫高的效果
        for (int i = 0; i < collectedYarnBalls.Count; i++)
        {
            Vector3 newPos = playerBase.position + new Vector3(0, i * yarnHeight, 0);
            collectedYarnBalls[i].transform.position = newPos;
        }
    }

    // 移除毛线球
    public void RemoveYarnBall()
    {
        if (collectedYarnBalls.Count > 0)
        {
            //从最下面开始移除
            GameObject removedBall = collectedYarnBalls[0];
            collectedYarnBalls.RemoveAt(0);
            Destroy(removedBall);

            UpdatePlayerHeight();
            PositionYarnBalls();
        }
    }

}
