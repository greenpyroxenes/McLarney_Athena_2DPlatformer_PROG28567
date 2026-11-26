using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;

    [Header("Horiztonal")]
    public float moveSpeed = 5f;
    public float accelerationTime = 0.15f;
    public float decelerationTime = 0.15f;
    private float acceleration;
    private float deceleration;
    private Vector2 velocity;

    [Header("Vertical")]
    public float apexHeight = 3f;
    public float apexTime = 0.3f;
    public float coyoteTime = 0.3f;
    private float gravity;
    private float jumpVelocity;
    private float terminalVelocity;

    bool isWalking;
    private float coyTimer = 0f;
    public bool grounded;
    public bool jumping;
    public bool coyoteTrigger = false;
    FacingDirection fd;
    BoxCollider2D coll;
    public LayerMask ground;

    public enum FacingDirection
    {
        left, right
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0;

        acceleration = moveSpeed / accelerationTime;
        deceleration = moveSpeed / decelerationTime;

        gravity = -2 * apexHeight / Mathf.Pow(apexTime, 2);
        jumpVelocity = 2 * apexHeight / apexTime;
        terminalVelocity = -1 * jumpVelocity;
    }


    void Update()
    {
        Vector2 playerInput = new Vector2();

        if (Keyboard.current.aKey.isPressed == true || Keyboard.current.leftArrowKey.isPressed == true)
        {
            playerInput.x -= 1;
            fd = FacingDirection.left;
        }
        if (Keyboard.current.dKey.isPressed == true || Keyboard.current.rightArrowKey.isPressed == true)
        {
            playerInput.x += 1;
            fd = FacingDirection.right;
        }
        if (!Keyboard.current.anyKey.isPressed)
        {
            playerInput.x = 0;
        }
        if(Keyboard.current.spaceKey.wasPressedThisFrame && grounded)
        {
            jumping = true;
            coyoteTrigger = true;
        }
        if (coyoteTrigger == true)
        {
            coyTimer += Time.deltaTime;
            if(coyTimer < coyoteTime && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumping = true;
            }
            if(coyTimer > coyoteTime)
            {
                coyoteTrigger = false;
                coyTimer = 0;
            }
        }
        if (Physics2D.BoxCast(coll.bounds.center, coll.size, 0, Vector2.down, 0.5f, ground))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        MovementUpdate(playerInput);

    }
    void FixedUpdate()
    {
        if(grounded == false)
        {
            velocity.y += gravity * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, terminalVelocity, velocity.y);
        }
        if(grounded == true)
        {
            velocity.y = 0;
        }
        if(jumping == true)
        {
            velocity.y = jumpVelocity;
            jumping = false;
        }

        rb.linearVelocity = velocity;
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        if (playerInput.x != 0)
        {
            velocity.x += playerInput.x * Time.deltaTime * acceleration;
            velocity.x = Mathf.Clamp(velocity.x, -moveSpeed, moveSpeed);
            isWalking = true;
        }
        if(playerInput.x == 0)
        {
            isWalking = false;
            velocity.x -= Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    public bool IsGrounded()
    {
        return grounded;
    }

    public FacingDirection GetFacingDirection()
    {
        return fd;
    }
}
