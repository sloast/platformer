using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float RunAccelerate;
    public float RunReduce;
    public float AirMult;
    public float MaxRunSpeed;
    public float FallSpeed;
    public float FallAccel;
    public float JumpSpeed;
    public float dashSpeed;
    bool canJump = true;
    float leftGroundBuffer = 0f;
    bool canAbility = true;
    float jumpBuffer = 0f;
    public float maxJumpBuffer = .1f;
    bool dashNextFrame = false;
    Vector2 dashDirection;

    public bool onGround;
    Rigidbody2D rb;


    void Start()
    {
        transform.position = new Vector3(-12.5f, -6.5f, 0f);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (Input.GetKeyDown("c"))
        {
            StartJump();
        }
        if (Input.GetKeyDown("x"))
        {
            StartDash();
        }
    }

    // FixedUpdate is called 10x per second
    void FixedUpdate()
    {
        // Default movement
        MoveHorizontal();
        MoveVertical();
        CheckGrounded();
        CheckBuffer();
    }

    void CheckGrounded()
    {
        
        bool temp = onGround;
        onGround = isGrounded();
        if (!temp && onGround) // Check if the player just landed
        {
            ResetAbilities();
        }
        else if (!onGround) // Check if the player just left the ground
        {
            if (temp)
            {
                leftGroundBuffer = .1f; // Give a short period of time for the player to jump
            } else {
                leftGroundBuffer -= Time.fixedDeltaTime;
                if (leftGroundBuffer <= 0)
                {
                    canJump = false;
                }
            }

        }
        CheckBuffer();
    }

    // Check whether an input is queued
    void CheckBuffer()
    {
        if (dashNextFrame)
        {
            Dash();
        }

        if (jumpBuffer > 0)
        {
            if (canJump)
            {
                Jump();
            } else {
                jumpBuffer -= Time.fixedDeltaTime;
            }
        }
    }

    // Queues a jump to be executed on the next FixedUpdate, or when the player next touches the ground.
    void StartJump()
    {
        jumpBuffer = maxJumpBuffer;
    }

    void Jump()
    {
        jumpBuffer = 0f;
        canJump = false;
        float speed_y = JumpSpeed;
        rb.velocity = new Vector2(rb.velocity.x, speed_y);
    }

    // Get arrow keys input in a more useful format
    int GetAxis(string axis)
    {
        int val = 0;
        if (axis == "x")
        {
            val += Input.GetKey("right") ? 1 : 0;
            val += Input.GetKey("left") ? -1 : 0;
        } else {                                 // axis == y
            val += Input.GetKey("up") ? 1 : 0;
            val += Input.GetKey("down") ? -1 : 0;
        }
        return val;
    }

    // Reset jump and dash ability after landing
    void ResetAbilities()
    {
        canAbility = true;
        canJump = true;
    }

    // For usage in a delayed Invoke()
    void ResetAbilitiesIfGrounded()
    {
        if (onGround)
        {
            ResetAbilities();
        }
    }

    void StartDash()
    {
        dashNextFrame = true;
        int inputX = GetAxis("x");
        int inputY = GetAxis("y");
        if (inputX == 0 && inputY == 0){
            inputX = 1;
        }
        dashDirection = new Vector2(inputX, inputY).normalized;
    }

    void Dash()
    {
        dashNextFrame = false;
        if (!canAbility) { return; }
        canAbility = false; // Stop the player from using until landing

        rb.velocity = dashDirection * dashSpeed;
        
        Invoke("ResetAbilitiesIfGrounded", .5f); // Allows it to reset without the player needing to jump
    }

    // Controls gravity
    void MoveVertical()
    {
        float speedY = rb.velocity.y;
        speedY = Mathf.Lerp(speedY, FallSpeed, FallAccel * Time.fixedDeltaTime); // 
        rb.velocity = new Vector2(rb.velocity.x, speedY);
    }

    void MoveHorizontal()
    {
        int inputX = GetAxis("x");
        float speedX = rb.velocity.x;
        float mult = 1;
        float max = MaxRunSpeed;
        if (!onGround)
        {
            mult = AirMult;
        }
        if (inputX * speedX > 0 && Mathf.Abs(inputX) < Mathf.Abs(speedX))
        {
            speedX = Mathf.Lerp(speedX, inputX * max, RunReduce * mult * Time.fixedDeltaTime);
        } else
        {
            speedX = Mathf.Lerp(speedX, inputX * max, RunAccelerate * mult * Time.fixedDeltaTime);
        }

        rb.velocity = new Vector2(speedX, rb.velocity.y);
    }

    bool isGrounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y-.25f), 
            new Vector2(.8f, .5f), 0f, Vector2.down, .1f, LayerMask.GetMask("Map"));
        return ray.transform != null;
    }
}
