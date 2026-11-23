using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制Player的横板2D行动
/// 已实现移动和跳跃
/// 
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerStamina))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("移动配置")]
    [SerializeField] private float moveSpeed = 5f;
    [Header("Sprint 配置")]
    [SerializeField, Min(1f)] private float sprintSpeedMultiplier = 1.5f;
    [SerializeField, Min(0f)] private float sprintStaminaPerSecond = 25f;
    [Header("跳跃配置")]
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private int maxAirJumps = 1; //最大空中跳跃次数
    [SerializeField]
    private float jumpBufferTime = 0.1f; //跳跃缓冲时间
    [SerializeField] 
    private float coyoteTime = 0.1f; //离开地面后的跳跃容错时间
    

    public bool IsGrounded { get; private set; }
    public bool IsSprinting { get; private set; }
    private PlayerInputReader2D inputReader;
    private PlayerStamina stamina;
    private PlayerGroundSensor2D groundSensor; //检测
    private Rigidbody2D rb;

    private float horizontalInput;
    private int airJumpsRemaining;
    private float jumpBufferTimeRemaining;
    private float coyoteTimeRemaining; //悬空时间剩余

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputReader = GetComponent<PlayerInputReader2D>();
        stamina = GetComponent<PlayerStamina>();
        groundSensor = GetComponentInChildren<PlayerGroundSensor2D>();
        airJumpsRemaining = maxAirJumps;
    }

    private void Update()
    {
        horizontalInput = inputReader.MoveInput.x;
        if (inputReader.ConsumeJumpPressed())
        {
            jumpBufferTimeRemaining = jumpBufferTime; // Reset jump buffer time when jump is pressed
        }

    }

    private void FixedUpdate()
    {
        jumpBufferTimeRemaining = Mathf.Max(0, 
            jumpBufferTimeRemaining - Time.fixedDeltaTime
        );
        IsGrounded = groundSensor.CheckGrounded();
        bool hasLanded = IsGrounded && rb.velocity.y<0.01f;
        if (hasLanded)
        {
            airJumpsRemaining = maxAirJumps;
            coyoteTimeRemaining = coyoteTime; 
            // Reset coyote time when grounded

        }
        else
        {
            
            coyoteTimeRemaining = Mathf.Max(0, coyoteTimeRemaining - Time.fixedDeltaTime);
        }


        Vector2 velocity = rb.velocity;
        bool wantsToSprint = inputReader.SprintHeld
            && Mathf.Abs(horizontalInput) > 0.01f;
        IsSprinting = wantsToSprint && stamina.TrySpend(
            sprintStaminaPerSecond * Time.fixedDeltaTime
        );

        float currentMoveSpeed = IsSprinting
            ? moveSpeed * sprintSpeedMultiplier
            : moveSpeed;
        velocity.x = horizontalInput * currentMoveSpeed;
        if(jumpBufferTimeRemaining > 0f)
        {
            if (coyoteTimeRemaining >0f)
            {
                velocity.y = jumpForce;
                coyoteTimeRemaining = 0f; // Reset coyote time after jumping
                jumpBufferTimeRemaining = 0f;
            }
            else if(airJumpsRemaining > 0)
            {
                velocity.y = jumpForce;
                airJumpsRemaining--;
                jumpBufferTimeRemaining = 0f;
            }
        }
        rb.velocity = velocity;
    }

    private void OnDisable()
    {
        IsSprinting = false;
        jumpBufferTimeRemaining = 0f; // Reset jump buffer time when the script is disabled
    }

}
