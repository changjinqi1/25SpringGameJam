using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrigger : MonoBehaviour
{
    public enum ObstacleType { Bathtub, Brush }  //add tags
    public ObstacleType obstacleType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TESTcollect collect = other.GetComponent<TESTcollect>();

            if (collect != null)
            {
                if (collect.HasYarnBalls())
                {
                    collect.RemoveYarnBall();
                    Debug.Log($"{obstacleType} hit: Removed one yarn ball.");
                }
                else
                {
                    Debug.Log($"{obstacleType} hit with NO yarn ¡ú Player dies.");
                    FindObjectOfType<CameraFollowUpOnly>()?.LoadNextScene();
                }
            }
        }
    }
}
