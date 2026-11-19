using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerController;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 5f;
    bool isWalking;
    bool isGrounded;
    FacingDirection fd;

    public enum FacingDirection
    {
        left, right
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isGrounded = true;
    }

    void FixedUpdate()
    {
        
        Vector2 playerInput = new Vector2();

        if (Keyboard.current.aKey.isPressed == true || Keyboard.current.leftArrowKey.isPressed == true)
        {
            playerInput += Vector2.left;
        }
        if(Keyboard.current.dKey.isPressed == true || Keyboard.current.rightArrowKey.isPressed == true)
        {
            playerInput += Vector2.right;
        }
        if(!Keyboard.current.anyKey.isPressed)
        {
            playerInput = Vector2.zero;
        }

        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.

        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        if (playerInput.x > 0)
        {
            rb.linearVelocity = playerInput * moveSpeed;
            isWalking = true;
            fd = FacingDirection.right;
        }
        if (playerInput.x < 0)
        {
            rb.linearVelocity = playerInput * moveSpeed;
            isWalking = true;
            fd = FacingDirection.left;
        }
        if(playerInput.x == 0)
        {
            isWalking = false;
            rb.linearVelocity = playerInput;
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }

    public FacingDirection GetFacingDirection()
    {
        return fd;
    }
}
