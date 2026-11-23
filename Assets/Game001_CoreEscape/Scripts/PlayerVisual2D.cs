using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制player的视觉朝向
/// </summary>

public class PlayerVisual2D : MonoBehaviour
{
    [SerializeField] private float facingThreshold = 0.1f; // 阈值，用于判断是否需要翻转角色 

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PlayerMovement2D playerMovement;
    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        rb = GetComponentInParent<Rigidbody2D>();
        playerMovement = GetComponentInParent<PlayerMovement2D>();   
    }

    private void Update()
    {
        float horizontalVelocity = rb.velocity.x;
        
        UpdateFacing(horizontalVelocity);
        UpdateAnimator(horizontalVelocity);

    }

    private void UpdateFacing(float horizontalVelocity)
    {
        if(horizontalVelocity>facingThreshold)
        {
            spriteRenderer.flipX = false; // 向右
        }
        else if(horizontalVelocity < -facingThreshold)
        {
            spriteRenderer.flipX = true; // 向左
        }
    }

    private void UpdateAnimator(float horizontalVelocity)
    {
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(horizontalVelocity));
        animator.SetBool("IsGrounded", playerMovement.IsGrounded);
        animator.SetFloat("VerticalSpeed", rb.velocity.y);
    }
}
