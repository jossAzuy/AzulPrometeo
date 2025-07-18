using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [Header("Health Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private FloatingFeedbackText floatingFeedback; // Asigna el prefab/script de feedback en el inspector
    private int currentHealth;

    [Header("Rythm Settings")]
    [SerializeField] public GameController gameController; // Asigna el GameController en el inspector

    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] public Transform target; // Asigna el Player en el inspector

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Feedback Settings")]
    [SerializeField] private FeedbackManager feedbackManager; // Asigna el FeedbackManager en el inspector
    [SerializeField] private Transform feedbackOrigin; // Posición de origen del feedback asignada en el inspector

    [Header("Enemy Variant")]
    public bool alwaysFollowPlayer = false;

    [Header("Drop Settings")]
    public bool dropCoinsOnDeath = false;
    public GameObject coinPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Solo moverse/atacar durante el beat
        if (gameController == null || gameController.rhythmSystem == null || !gameController.rhythmSystem.IsPerfectBeat())
            return;

        if (target == null)
        {
            Patrol();
            return;
        }

        if (alwaysFollowPlayer)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer > attackRange)
            {
                FollowPlayer();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= visionRange)
            {
                if (distanceToPlayer > attackRange)
                {
                    FollowPlayer();
                }
                else
                {
                    Attack();
                }
            }
            else
            {
                Patrol();
            }
        }
    }

    private void FollowPlayer()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform patrolTarget = patrolPoints[currentPatrolIndex];
        Vector3 direction = (patrolTarget.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(transform.position, patrolTarget.position);
        if (distance < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void Attack()
    {
        // Daño al jugador si tiene el componente PlayerHealth
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        // Debug.Log("Enemigo ataca al jugador y causa " + damage + " de daño.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage); // Usar el daño de la bala
                Destroy(collision.gameObject); // Destruir la bala
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (feedbackManager != null && floatingFeedback != null && feedbackOrigin != null)
        {
            feedbackManager.ShowFeedback(floatingFeedback, feedbackOrigin,amount.ToString());
        }
        // Debug.Log($"Salud actual del enemigo después de recibir daño: {currentHealth}");
        if (currentHealth <= 0)
        {
            if (dropCoinsOnDeath && coinPrefab != null)
            {
                int coins = Random.Range(1, 4);
                for (int i = 0; i < coins; i++)
                {
                    Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
                    Instantiate(coinPrefab, transform.position + offset, Quaternion.identity);
                }
            }
            Destroy(gameObject);
        }
    }

    public void Setup(GameController gc, Transform tgt, FeedbackManager fbManager, FloatingFeedbackText floatingText)
    {
        gameController = gc;
        target = tgt;
        feedbackManager = fbManager;
        floatingFeedback = floatingText;
    }
}
