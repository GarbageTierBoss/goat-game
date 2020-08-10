using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection { Left, Right }

    [SerializeField] private float m_jumpApexHeight;
    [SerializeField] private float m_jumpApexTime;
    [SerializeField] private float m_terminalVelocity;
    /*[SerializeField]*/ private float m_coyoteTime;
    /*[SerializeField]*/ private float m_jumpBufferTime;
    [SerializeField] private float m_accelerationTimeFromRest;
    [SerializeField] private float m_decelerationTimeToRest;
    [SerializeField] private float m_maxHorizontalSpeed;
    /*[SerializeField]*/ private float m_accelerationTimeFromQuickturn;
    /*[SerializeField]*/ private float m_decelerationTimeFromQuickturn;

    Rigidbody2D rb;         //rigidbody component
    Vector2 prevInput;      //tracking previous horizontal input
    Vector2 spriteScale;    //scale of the sprite used for changing facing direction

    float m_timeElapsed;    //time elapsed since jump was hit
    float m_terminalTime;   //time until reaching terminal velocity
    float m_initJumpTime;   //time until reaching initial jump velocity
    float m_postJumpTime;   //time until zero velocity after key release

    float m_bufferElapsed;  //
    float m_coyoteElapsed;

    float jumpSpeed;    //speed of jump velocity
    float fallSpeed;    //speed of fall velocity
    float walkSpeed;    //speed of walk velocity

    bool jumping;   //checks if player is jumping (rising)
    bool quickTurn; //true if quick turn is in effect
    bool turned;    //checks if player has turned during a quick turn
    bool buffered;  //

    bool falling = true;
    bool onGround = false;

    const float gravity = 0.75f;
    const float runAccel = 0.9f;
    const float initJump = 21.3f;
    const float midJump = 17.1f;

    bool isInitialJump = false;
    bool isMidJump = false;
    bool isApexJump = false;

    GameObject ground;
    Collider2D hit;

    public bool IsWalking()
    {
        if (walkSpeed != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void StopWalking()
    {
        walkSpeed = 0;
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(rb.position/* + new Vector2(0, rb.velocity.y)*/, GetComponent<BoxCollider2D>().size, 0, Vector2.down, fallSpeed * Time.fixedDeltaTime, LayerMask.GetMask("Ground"));

        if (hit)
        {
            ground = hit.collider.gameObject;
        }

        return hit;
    }

    public FacingDirection GetFacingDirection()
    {
        if (spriteScale.x > 0)
        {
            return FacingDirection.Right;
        }
        else
        {
            return FacingDirection.Left;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        spriteScale = GetComponentInChildren<SpriteRenderer>().transform.localScale;

        jumpSpeed = 0;
        fallSpeed = 0;
        walkSpeed = 0;

        jumping = false;    //true if currently jumping (false when falling)
        quickTurn = false;  //true if quick turn is in progress
        turned = false;     //true if player has turned during a quick turn
        buffered = false;

        m_timeElapsed = 0;          //in seconds

        //ORIGINAL VALUES

        /*m_terminalTime = 1f / 4f;   //in seconds
        m_initJumpTime = 1f / 60f;  //in seconds
        m_postJumpTime = 1f / 5f;   //in seconds

        m_bufferElapsed = 0;    //in seconds
        m_coyoteElapsed = 0;    //in seconds

        m_jumpApexHeight = 4.295f;                  //in tiles
        m_jumpApexTime = 1f / 6f;                   //in seconds
        m_jumpBufferTime = 1f / 6f;                 //in seconds
        m_coyoteTime = 1f / 6f;                     //in seconds
        m_decelerationTimeToRest = 1f / 6f;         //in seconds
        m_accelerationTimeFromRest = 1f / 5f;       //in seconds
        m_accelerationTimeFromQuickturn = 1f / 30f; //in seconds
        m_decelerationTimeFromQuickturn = 2f / 15f; //in seconds
        m_terminalVelocity = 27.6f;                 //in tiles per second
        m_maxHorizontalSpeed = 5.85f;               //in tiles per second*/
    }

    private void Update()
    {
        hit = Physics2D.OverlapBox(rb.position - (GetComponent<BoxCollider2D>().size / 2) + (GetComponent<BoxCollider2D>().size / 20), GetComponent<BoxCollider2D>().size / 10, 0, LayerMask.GetMask("Ground"));
        
        CheckJump();
    }

    private void FixedUpdate()
    {
        Gravity();
        JumpSim();

        if (!quickTurn)
        {
            CheckMove();    //move normally if not quick turning
        }

        m_timeElapsed += Time.fixedDeltaTime;
        m_bufferElapsed += Time.fixedDeltaTime;
        m_coyoteElapsed += Time.fixedDeltaTime;
    }

    void CheckMove()
    {
        Vector2 temp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (temp.x != 0)
        {
            Vector2 xInput = new Vector2(temp.x, 0); //ignores y-axis input

            if (!IsGrounded())
            {
                rb.position += xInput * m_maxHorizontalSpeed * Time.fixedDeltaTime;
            }
            else
            {
                walkSpeed = AccelerateWithTime(xInput, m_accelerationTimeFromRest, 0, m_maxHorizontalSpeed, walkSpeed);
            }

            if (transform.localScale.x != xInput.x)
            {
                transform.localScale = new Vector3(xInput.x, 1, 1);
                Debug.Log("turn");
            }

            prevInput = xInput;
            //Debug.Log(ShovelKnightInput.GetDirectionalInput());
        }
        else
        {
            walkSpeed = AccelerateWithTime(prevInput, m_decelerationTimeToRest, m_maxHorizontalSpeed, 0, walkSpeed);
        }
    }

    float AccelerateWithTime(Vector2 direction, float duration, float init, float final, float current)
    {
        float max = final > init ? final : current;

        current = Mathf.Clamp(current + ((final - init) / duration * Time.fixedDeltaTime), 0, max);
        rb.position += direction * current * Time.fixedDeltaTime;

        //Debug.Log(current);
        return current;
    }

    void Gravity()
    {
        if (!IsGrounded() && !jumping)
        {
            onGround = false;
            fallSpeed = AccelerateWithVelocity(Vector2.down, fallSpeed, gravity, 0, m_terminalVelocity);
        }
        else
        {
            //TODO: WHILE FALLING
            if (falling)
            {
                //IMPORTANT
                /*rb.position = new Vector2(rb.position.x, (ground.transform.position.y + ground.GetComponent<BoxCollider2D>().bounds.extents.y + GetComponent<BoxCollider2D>().size.y / 2)- 0.001f);*/
                
                falling = false;
                //end while
            }

            fallSpeed = 0;
        }
    }
    
    void JumpSim()
    { 
        if (isInitialJump) { jumpSpeed = AccelerateWithVelocity(Vector2.up, jumpSpeed, initJump, 0, initJump); } 
        else if (isMidJump)
        {
            jumpSpeed = AccelerateWithVelocity(Vector2.up, jumpSpeed, midJump, 0, midJump);
        } 
        else if (isApexJump)
        {
            jumpSpeed = AccelerateWithVelocity(Vector2.up, jumpSpeed, -1, 0, midJump);
        }
    }

    void CheckJump()
    {
        if (hit)
        {
            onGround = true;
            isInitialJump = false;
            isMidJump = false;
            isApexJump = false;
        }

        if (Input.GetButton("Jump") && Input.GetButtonDown("Jump")
            && onGround && !jumping)
        {
            isInitialJump = true;
            isMidJump = false;
            isApexJump = false;
            m_timeElapsed = 0;
            jumping = true;
            onGround = false;
        }
        else if (Input.GetButton("Jump") && !Input.GetButtonDown("Jump")
            && jumping && m_timeElapsed <= m_jumpApexTime)
        {
            isInitialJump = false;
            isMidJump = true;
            isApexJump = false;
        }
        else if (((!Input.GetButtonDown("Jump") && !Input.GetButton("Jump")) || m_timeElapsed > m_jumpApexTime))
        {
            StopJump();
        }
    }

    public void StopJump()
    {
        isInitialJump = false;
        isMidJump = false;
        isApexJump = true;
        jumping = false;
    }

    float AccelerateWithVelocity(Vector2 input, float velocity, float accel, float min, float max)
    {
        Vector2 prev = rb.position;

        velocity = Mathf.Clamp(velocity + accel, min, max);
        rb.position += input * velocity * Time.fixedDeltaTime;

        Vector2 temp = rb.position;

        if (temp.y - prev.y < 0)
        {
            falling = true;
        }

        return velocity;
    }
    
    /*void Gravity()
    {
        if (!IsGrounded() && !jumping)
        {
            m_timeElapsed = 0;
            fallSpeed = Accelerate(Vector2.down, m_terminalTime, 0, m_terminalVelocity, fallSpeed);
        }
        else
        {
            fallSpeed = 0;
        }

        //Debug.Log(IsGrounded());
    }

    void CheckQuickTurn()
    {
        Vector2 temp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //if quickturn is in effect or if the conditions are met for quick turn, adjust new acceleration values
        if ((prevInput.x != temp.x && walkSpeed != 0 //previous input must not be the same as current input
            && temp.x != 0) || quickTurn)    //walk speed must not be 0, and horizontal input must not be 0
        {
            quickTurn = true;
            Vector2 xInput = new Vector2(temp.x, 0); //ignores y-axis input

            if (walkSpeed == 0)
            {
                turned = true;
            }

            if (turned && IsGrounded())
            {
                walkSpeed = AccelerateWithTime(xInput, m_accelerationTimeFromQuickturn, 0, m_maxHorizontalSpeed, walkSpeed);
            }
            else if (IsGrounded())
            {
                walkSpeed = AccelerateWithTime(xInput, m_decelerationTimeFromQuickturn, m_maxHorizontalSpeed, 0, walkSpeed);
            }
            else
            {
                rb.position += xInput * m_maxHorizontalSpeed * Time.fixedDeltaTime;
            }
        }

        if ((walkSpeed == 0 && temp.x == 0) || walkSpeed == m_maxHorizontalSpeed)
        {
            quickTurn = false;
            turned = false;
        }
    }

    void CheckJump()
    {
        if (ShovelKnightInput.WasJumpPressed() && ShovelKnightInput.IsJumpPressed() && IsGrounded() && !jumping)
        {
            jumpSpeed = Accelerate(Vector2.up, m_initJumpTime, 0, initJump, jumpSpeed);
            m_timeElapsed = 0;
            jumping = true;

            Debug.Log("Init.");
        }
        else if (!ShovelKnightInput.WasJumpPressed() && ShovelKnightInput.IsJumpPressed() && jumping && m_timeElapsed < m_jumpApexTime)
        {
            jumpSpeed = Accelerate(Vector2.up, m_jumpApexTime, initJump, midJump, jumpSpeed);
            m_holdElapsed = 0;
        }
        else if (((!ShovelKnightInput.WasJumpPressed() && !ShovelKnightInput.IsJumpPressed()) || m_timeElapsed > m_jumpApexTime))
        {
            jumpSpeed = Accelerate(Vector2.up, m_postJumpTime, midJump, 0, jumpSpeed);
            jumping = false;
        }
    }

    void CheckMove()
    {
        if (ShovelKnightInput.GetDirectionalInput() != Vector2.zero && ShovelKnightInput.GetDirectionalInput().y == 0)
        {
            walkSpeed = Accelerate(ShovelKnightInput.GetDirectionalInput(), walkSpeed, runAccel, 0, m_maxHorizontalSpeed);
        }
        else
        {
            walkSpeed = Accelerate(ShovelKnightInput.GetDirectionalInput(), walkSpeed, -runAccel, 0, m_maxHorizontalSpeed);
        }
    }

    void AirMove()
    {
        if (ShovelKnightInput.GetDirectionalInput() != Vector2.zero && ShovelKnightInput.GetDirectionalInput().y == 0)
        {
            rb.position += ShovelKnightInput.GetDirectionalInput() * m_maxHorizontalSpeed * Time.fixedDeltaTime;
        }
    }*/
}
