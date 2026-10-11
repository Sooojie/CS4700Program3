using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float lifetime = 1f;

    private Rigidbody2D rb;
    private int direction = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(int facingDirection)
    {
        direction = facingDirection;
    }

    private void Start()
    {
        rb.linearVelocity = Vector2.right * direction * speed;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.LogWarning(other);
        Destroy(gameObject);
    }
}
