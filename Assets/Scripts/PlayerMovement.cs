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
    public bool canJump;
    public bool canAbility;

    public bool onGround;
    Rigidbody2D rb;


    void Start()
    {
        transform.position = new Vector3(-12.5f, -6.5f, 0f);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        // Jump
        if (onGround && Input.GetKeyDown("c"))
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpSpeed);
        }
    }

    // FixedUpdate is called 10x per second
    void FixedUpdate()
    {
        // Default movement
        MoveHorizontal();
        MoveVertical();

        // Check if the player just landed
        bool temp = onGround;
        onGround = isGrounded();
        if (!temp && onGround)
        {
            ResetAbilities();
        }
        
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

    void Dash()
    {
        canAbility = false; // Stop the player from using until landing
        rb.velocity = new Vector2(rb.velocity.x, JumpSpeed); // Currently just a second jump
        Invoke("ResetAbilitiesIfGrounded", 1); // Allows it to reset without the player needing to jump
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
