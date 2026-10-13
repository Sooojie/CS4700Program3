using UnityEngine;

/// <summary>
/// Moves a platform back and forth and attaches the player when collided.
/// Attach this script to the Platform GameObject.
/// </summary>
public class SlidingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveDirection = new Vector3(1f, 0f, 0f); // Movement direction
    [SerializeField] private float moveDistance = 5f; // How far to move
    [SerializeField] private float moveSpeed = 2f;    // Speed of movement

    private Vector3 startPosition;
    private bool movingForward = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        // Calculate target position
        Vector3 targetPosition = startPosition + (movingForward ? moveDirection.normalized * moveDistance : -moveDirection.normalized * moveDistance);

        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Reverse direction when reaching target
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            movingForward = !movingForward;
        }
    }

    // Attach player to platform when collision occurs
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player stepped on moving platform: {collision.gameObject.name}");
            collision.transform.SetParent(transform); // Make player a child of platform
        }
    }

    // Detach player when leaving platform
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player left moving platform: {collision.gameObject.name}");
            collision.transform.SetParent(null); // Remove parent relationship
        }
    }
}
