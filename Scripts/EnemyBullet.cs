using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;
    private Rigidbody2D rb;
    private bool hasCollided = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (hasCollided) return;



        // Daño solo si el objeto tiene el tag Player

        if (collision.CompareTag("Player"))
        {
            hasCollided = true;
            Debug.Log("Enemy bullet hit the player!");
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
            return;
        }

        // Si choca con algo que no sea el jugador ni otra bala enemiga, destrúyela
        if (!collision.CompareTag("Enemy") && !collision.CompareTag("EnemyBullet"))
        {
            hasCollided = true;
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
