using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingStoneHit : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.collider.gameObject;

        if (hitObject.CompareTag("Player"))
        {
            HandlePlayerHit(hitObject);
        }
        else if (hitObject.CompareTag("YarnBall"))
        {
            // œÚ…œ’“ Player
            Transform current = hitObject.transform;
            while (current != null && !current.CompareTag("Player"))
            {
                current = current.parent;
            }

            if (current != null && current.CompareTag("Player"))
            {
                HandlePlayerHit(current.gameObject);
            }
        }
    }

    void HandlePlayerHit(GameObject player)
    {
        TESTcollect collect = player.GetComponent<TESTcollect>();

        if (collect != null)
        {
            if (collect.HasYarnBalls())
            {
                collect.RemoveYarnBall();
                Debug.Log("RollingStone hit: removed one yarn ball.");
            }
            else
            {
                Debug.Log("RollingStone hit: no yarn balls, player dies.");
                FindObjectOfType<CameraFollowUpOnly>()?.LoadNextScene();
            }
        }
    }
}
