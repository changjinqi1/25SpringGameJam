using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnYarnCollected()
    {
        // Play JumpOnBall
        animator.SetTrigger("DoJump");
        animator.SetBool("HasYarn", true);
    }

    public void OnYarnListEmpty()
    {
        // Play PlayerRun when have NO yarn ball
        animator.SetBool("HasYarn", false);
    }
}
