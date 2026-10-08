using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Camera Settings")]
    [SerializeField] private float smoothSpeed = 8f;

    [Header("Level Boundaries")]
    [SerializeField] private BoxCollider2D levelBounds;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (player == null || levelBounds == null)
        {
            return;
        }

        // Get the boundaries of the level.
        Bounds bounds = levelBounds.bounds;

        // Calculate camera height
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // Center the camera on the player
        Vector3 targetPosition = new Vector3(
            player.position.x,
            player.position.y,
            transform.position.z
        );

        // Stop camera from going outside the level
        if (bounds.size.x <= halfWidth * 2f)
        {
            targetPosition.x = bounds.center.x;
        }
        else
        {
            targetPosition.x = Mathf.Clamp(
                targetPosition.x,
                bounds.min.x + halfWidth,
                bounds.max.x - halfWidth
            );
        }

        if (bounds.size.y <= halfHeight * 2f)
        {
            targetPosition.y = bounds.center.y;
        }
        else
        {
            targetPosition.y = Mathf.Clamp(
                targetPosition.y,
                bounds.min.y + halfHeight,
                bounds.max.y - halfHeight
            );
        }

        // Smoothly move the camera.
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
