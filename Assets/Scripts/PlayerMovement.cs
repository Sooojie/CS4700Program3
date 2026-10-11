using System.Threading;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float horizontalInput;
    public bool airborne;
    private bool jumpPressed;
    private bool isShooting;
    private double shootTimer = 0.0;
    Animator animator;

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

        if (Keyboard.current.gKey.wasPressedThisFrame)
            {
                isShooting = true;
                shootTimer = 1.2;
            }
            else
            {
                if (shootTimer > 0)
                {
                    shootTimer = shootTimer - .01;
                }
                else
                {
                isShooting = false;
                }
            }
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
                    if (isShooting)
                    animator.Play("Player_RunShoot");
                    else
                    animator.Play("Player_Run");
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
                    if (isShooting)
                    animator.Play("Player_RunShoot");
                    else
                    animator.Play("Player_Run");
                }
            }

            // Jump using Space.
            else if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                jumpPressed = true;
                if (isShooting)
                animator.Play("Player_JumpShoot");
                else
                animator.Play("Player_Jump");
            }
            else if (isGrounded)
            {
                if (isShooting)
                animator.Play("Player_Shoot");
                else
                animator.Play("Player_Idle");
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
