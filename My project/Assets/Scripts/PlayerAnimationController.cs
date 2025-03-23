using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private TESTcollect collectScript;

    void Start()
    {
        animator = GetComponent<Animator>();
        collectScript = GetComponentInParent<TESTcollect>();
    }

    void Update()
    {
        if (collectScript != null && animator != null)
        {
            int count = collectScript.GetYarnBallCount();
            animator.SetInteger("yarnBallCount", count);
        }
    }

    public void OnYarnCollected()
    {
        if (animator != null)
        {
            animator.SetTrigger("DoJump");
        }
    }

    public void OnYarnListEmpty()
    {
        if (animator != null)
        {
            animator.SetInteger("yarnBallCount", 0);
        }
    }
}
