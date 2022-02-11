using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float RunAccelerate;
    public float RunReduce;
    public float AirMult;
    public float MaxRunSpeed;
    public float FallSpeed;
    public float wallSlideSpeed;
    public float wallHopSpeed;
    public float climbSpeed;
    public float fallAccel;
    public float JumpSpeed;
    public float dashSpeed;
    public Vector2 wallJumpSpeed;
    bool canJump = true;
    float leftGroundBuffer = 0f;
    bool canAbility = true;
    float jumpBuffer = 0f;
    public float maxJumpBuffer = .1f;
    bool dashNextFrame = false;
    Vector2 dashDirection;

    bool onGround = true;
    bool onWall = false;
    int wallDirection = 0;
    bool climbing = false;
    bool canClimbDown = true;
    bool canMoveHorizontal = true;
    Vector2 startCoordinates;
    Rigidbody2D rb;
    SpriteRenderer rend;


    void Start()
    {
        transform.position = new Vector3(-12.5f, -6.5f, 0f);
        startCoordinates = new Vector3(-12.5f, -6.5f, 0f);
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
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
        
        CheckGrounded(); // Update state
        CheckWall();
        
        if (canMoveHorizontal) { MoveHorizontal(); } // Default movement
        MoveVertical();
        CheckBuffer();
        SetDirection();
    }

    void SetDirection()
    {
        float speedX = rb.velocity.x;
        if (speedX > .1f && rend.flipX)
        {
            rend.flipX = false;
        } else if (speedX < -.1f && !rend.flipX)
        {
            rend.flipX = true;
        }
    }

    void onDied()
    {
        transform.position = startCoordinates;
        rb.velocity = Vector3.zero;
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
    }

    void CheckWall()
    {
        int temp = wallDirection;
        wallDirection = isOnWall();
        onWall = wallDirection != 0;
        if (temp != 0 && !onWall && isOnWallTop(temp))
        {
            HopWall();
        }
        if (Input.GetKey("z") && onWall)
        {
            climbing = true;
            if (isOnWallBottom(wallDirection))
            {
                canClimbDown = false;
            } else if (!canClimbDown) {
                canClimbDown = true;
            }

        } else if (climbing) {
            climbing = false;
        }
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
            } else if (onWall) {
                WallJump();
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

    void WallJump()
    {
        jumpBuffer = 0f;
        canJump = false;
        float speedX = wallJumpSpeed.x * -wallDirection;
        float speedY = wallJumpSpeed.y;
        rb.velocity = new Vector2(speedX, speedY);
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
        if (climbing)
        {
            MoveVertical_climbing();
            return;
        }
        float speedY = rb.velocity.y;
        speedY = Mathf.Lerp(speedY, -FallSpeed, fallAccel * Time.fixedDeltaTime);
        if (onWall && speedY < -wallSlideSpeed)
        {
            speedY = -wallSlideSpeed;
        }
        rb.velocity = new Vector2(rb.velocity.x, speedY);
    }

    void MoveVertical_climbing()
    {
        float speedY = GetAxis("y") * climbSpeed;
        if (!canClimbDown && speedY < 0)
        {
            speedY = 0;
        }
        rb.velocity = new Vector2(rb.velocity.x, speedY);
    }

    // default movement
    void MoveHorizontal()
    {
        if (climbing)
        {
            MoveHorizontal_climbing();
            return;
        }
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

    void MoveHorizontal_climbing()
    {
        float speedX = wallDirection * .2f;
        rb.velocity = new Vector2(speedX, rb.velocity.y);
    }

    bool isGrounded()
    {
        RaycastHit2D ray = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y-.25f), 
            new Vector2(.8f, .5f), 0f, Vector2.down, .1f, LayerMask.GetMask("Map"));
        return ray.transform != null;
    }

    int isOnWall()
    {
        RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, .6f, LayerMask.GetMask("Map"));
        RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, .6f, LayerMask.GetMask("Map"));
        int value = 0;
        if (left.transform != null)
        {
            value -= 1;
        } if (right.transform != null)
        {
            value += 1;
        }
        return value;
    }

    bool isOnWallTop(float side)
    {
        Vector3 offset = new Vector3(0, -.3f, 0);
        Vector2 direction = side == 1 ? Vector2.right : Vector2.left;
        RaycastHit2D ray = Physics2D.Raycast(transform.position + offset, direction, .6f, LayerMask.GetMask("Map"));
        return ray.transform != null;
    }

    bool isOnWallBottom(float side)
    {
        Vector3 offset = new Vector3(0, -.5f, 0);
        Vector2 direction = side == 1 ? Vector2.right : Vector2.left;
        RaycastHit2D ray = Physics2D.Raycast(transform.position + offset, direction, .6f, LayerMask.GetMask("Map"));
        return ray.transform == null;
    }

    // Jump up onto a platform from the wall below
    void HopWall()
    {
        float speedY = wallHopSpeed;
        rb.velocity = new Vector2(rb.velocity.x, speedY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacles")
        {
            onDied();
        }
    }
}
