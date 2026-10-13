/***************************************************************
*file: PlayerMovement.cs
*author:
*class: CS 4700 – Game Development
*assignment: program 3
*date last modified: 10/12/2026
*
*purpose: This program lets the user control the movements of the
*player sprite and adds speed and gravity to movement
*
****************************************************************/

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


    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;
    
    [Header("Character Facing")]
    public int FacingDirection { get; private set; } = 1;
    
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool jumpPressed;
    private string movementState;
    private Animator animator;
    private bool isGrounded;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireOffsetX = 0.7f;
    [SerializeField] private float fireOffsetY = 0.1f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Read keyboard input
        horizontalInput = 0f;

        Movement();

        if (Keyboard.current.gKey.isPressed)
        {
            Shoot();
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

    private void Movement()
    {
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
    }

    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab is not assigned!");
            return;
        }

        int direction = FacingDirection;
        Vector3 spawnPosition = transform.position + new Vector3(fireOffsetX * direction, fireOffsetY, 0f);

        GameObject bullet = Instantiate(
            bulletPrefab,
            spawnPosition,
            Quaternion.identity
        );

        // Set bullet direction
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDirection(direction);


        if (movementState == "Jumping")
        {
            animator.Play("Player_JumpShoot");
        }
        else if (movementState == "Running")
        {
            animator.Play("Player_RunShoot");
        }
        else
        {
            animator.Play("Player_Shoot");
        }

    }
}
