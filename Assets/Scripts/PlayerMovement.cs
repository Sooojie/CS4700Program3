using System.Threading;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float horizontalInput;
    private bool jumpPressed;
    public string movementState;

    public Animator animator;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    
    private bool isGrounded;

    [Header("Character Facing")]
    public int FacingDirection { get; private set; } = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Read keyboard input
        horizontalInput = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                if (FacingDirection == 1)
                {
                    transform.Rotate(0.0f, 180.0f, 0.0f);
                }
                horizontalInput = -1f;
                if (isGrounded)
                {
                    animator.Play("Player_Run");
                    movementState = "Running";
                }
            }

            else if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                horizontalInput = 1f;
                if (FacingDirection == -1)
                {
                    transform.Rotate(0.0f, 180.0f, 0.0f);
                }
                if (isGrounded)
                {
                    animator.Play("Player_Run");
                    movementState = "Running";
                }
            }

            // Jump using Space.
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumpPressed = true;
                animator.Play("Player_Jump");
                movementState = "Jumping";
            }
            else if (isGrounded)
            {
                animator.Play("Player_Idle");
                movementState = "Idle";
            }
        }

        // If horizontalInput = -1f; FacingDirection = -1f, horizontalInput = 1f; FacingDirection = 1f
        if (horizontalInput != 0)
        {
            FacingDirection = horizontalInput > 0 ? 1 : -1;
        }

        // Check if player is touching the ground
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );
    }

    private void FixedUpdate()
    {
        // Move left and right.
        rb.linearVelocity = new Vector2(
            horizontalInput * moveSpeed,
            rb.linearVelocity.y
        );

        // Jump only when grounded.
        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }

        jumpPressed = false;
    }

    // Ground check radius 
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                groundCheck.position,
                groundCheckRadius
            );
        }
    }
}
