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
        
        animator.SetTrigger("DoJump");
    }

    public void OnYarnListEmpty()
    {
        
        animator.Play("PlayerRun");
    }
}
