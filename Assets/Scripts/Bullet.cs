using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Velocidad de la bala
    public int damage = 1; // Daño que inflige la bala
    private Rigidbody2D rb;
    private bool hasCollided = false; // Bandera para evitar múltiples colisiones

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

 /*    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCollided) return; // Evitar múltiples colisiones

        hasCollided = true; // Marcar como colisionado

        // Aplicar daño si colisiona con un enemigo
        EnemyAI enemy = collision.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"Daño infligido al enemigo: {damage}");
            Destroy(gameObject); // Destruir la bala después de aplicar el daño
        }
    } */

    void OnBecameInvisible()
    {
        // Destruir la bala cuando salga de la pantalla
        Destroy(gameObject);
    }
}