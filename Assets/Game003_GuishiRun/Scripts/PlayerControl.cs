using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [Header("运动参数")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;
    [SerializeField]
    private float dashForce = 15f;

    [SerializeField]
    private float shiftHoldThreshold = 0.18f;

    [Header("跳跃参数")]
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private int maxJumpCount = 2;
    

    [Header("地面检测")]
    [SerializeField]
    private GroundCheck groundCheck;

    [Header("攀爬参数")]
    [SerializeField]
    private float climbSpeed = 5f;
    [SerializeField]
    private RopperClimber ropperClimber;





    private Rigidbody2D rb;
    private int moveIndirection = 0;

    private bool isGrounded;
    private bool isJumping = false;
    private int jumpCount = 0;

    private bool isRunning = false;
    private bool isDashing = false;
    private float leftshiftTimeAt = 0f;
    //攀爬相关
    private int climbDirection = 0;
    private bool climbRequest = false;
    private bool isClimbing = false;
    private bool nearbyRope = false;
    private float gravityScaleAtStart;





    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityScaleAtStart = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        ReadMoveInput();
        ReadShiftInput();
        ReadClimbInput();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

    }

    void FixedUpdate()
    {
        isGrounded = groundCheck.IsGrounded();
        if(isGrounded && rb.velocity.y <= 0f)
        {
            jumpCount = 0;
        }

        if(isRunning)
        {
            Run();
        }
        else if(isDashing)
        {
            Dash();
        }
        else
        {
            Move();
        }
       
        if(isJumping)
        {
            Jump();
        }


        if(climbRequest){
            if(isClimbing)
            {
                ExitClimb();
                climbRequest = false;
            }
            else if(nearbyRope && !isClimbing && moveIndirection.Equals(0))
            {
                EnterClimb();
                climbRequest = false;
            }
        }
        if(isClimbing)
        {
            Climb();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("lapper"))
        {
            nearbyRope = true;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("lapper"))
        {
            nearbyRope = false;
        }
    }

    void ReadMoveInput()
    {
        int right = Input.GetKey(KeyCode.D)? 1:0;
        int left = Input.GetKey(KeyCode.A)? 1:0;
        moveIndirection = right - left;
    }
    void ReadShiftInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            leftshiftTimeAt = Time.time;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            float leftshiftHoldTime = Time.time - leftshiftTimeAt;
            if(leftshiftHoldTime >= shiftHoldThreshold)
            {
                isRunning = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {   
            isRunning = false;
            float leftshiftHoldTime = Time.time - leftshiftTimeAt;
            if(leftshiftHoldTime < shiftHoldThreshold)
            {
                isDashing = true;
            }
            else
            {
                isDashing = false;
            }
        }
        
    }
    
    
    void ReadClimbInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            climbRequest = true;
        }

        int up = Input.GetKey(KeyCode.W)? 1:0;
        int down = Input.GetKey(KeyCode.S)? 1:0;
        climbDirection = up - down;

    }
    void Move()
    {
        Vector2 playermove = rb.velocity;
        playermove.x = moveIndirection * moveSpeed;
        rb.velocity = playermove;
    }

    void Run()
    {
        Debug.Log("快跑");
        Vector2 playerrun = rb.velocity;
        playerrun.x = moveIndirection * runSpeed;
        rb.velocity = playerrun;
    }

    void Dash()
    {
        Debug.Log("冲刺");
        Vector2 playerdash = rb.velocity;
        playerdash.x = moveIndirection * dashForce;
        playerdash.y = 0f;
        rb.velocity = playerdash;
        isDashing = false;
        
    }

    void Jump()
    {   bool canJump = jumpCount < maxJumpCount || isGrounded;
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
        isJumping = false;
    }

    void EnterClimb()
    {
        Debug.Log("进入攀爬");
        isClimbing = true;
        rb.gravityScale = 0f;
    }

    void Climb()
    {
        if(rb.position.y >= ropperClimber.ropeTop.position.y && climbDirection > 0)
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.position = ropperClimber.printPlayer.position;
            ExitClimb();
            return;
        }
        rb.velocity = new Vector2(0f, climbDirection * climbSpeed);
    }

    void ExitClimb()
    {
        Debug.Log("退出攀爬");
        isClimbing = false;
        rb.gravityScale = gravityScaleAtStart;
        climbRequest = false;
    }
}
