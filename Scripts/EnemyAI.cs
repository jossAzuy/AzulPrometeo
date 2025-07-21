using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Dash Variant")]
    [SerializeField] public bool canDash = false;
    [SerializeField] private float dashSpeed = 8f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    private float dashTimer = 0f;
    private float dashTimeLeft = 0f;
    private bool isDashing = false;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private FloatingFeedbackText floatingFeedback; // Asigna el prefab/script de feedback en el inspector
    private int currentHealth;

    [Header("Rythm Settings")]
    public GameController gameController; // Asigna el GameController en el inspector

    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private int damage = 1;
    private Transform target; // Asigna el Player en el inspector

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Feedback Settings")]
    private FeedbackManager feedbackManager; // Asigna el FeedbackManager en el inspector
    private Transform feedbackOrigin; // Posición de origen del feedback asignada en el inspector

    [Header("Enemy Variant")]
    public bool alwaysFollowPlayer = false;

    [Header("Drop Settings")]
    public bool dropCoinsOnDeath = false;
    public GameObject coinPrefab;

    [Header("Area Shoot Variant")]
    [SerializeField] public bool canShootArea = false;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int bulletsPerShot = 8;
    [SerializeField] private float shootCooldown = 2f;
    // [SerializeField] private float bulletSpeed = 6f;
    private float shootTimer = 0f;

    [Header("Death Effects")]
    [SerializeField] private GameObject deathParticlesPrefab; // Prefab del efecto de partículas al morir

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Solo moverse/atacar/disparar/dash durante el beat
        if (gameController == null || gameController.rhythmSystem == null || !gameController.rhythmSystem.IsPerfectBeat())
            return;

        // Dash
        if (canDash)
        {
            if (isDashing)
            {
                DashTowardsPlayer();
                dashTimeLeft -= Time.deltaTime;
                if (dashTimeLeft <= 0f)
                {
                    isDashing = false;
                    dashTimer = dashCooldown;
                }
                return;
            }
            else
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0f && target != null)
                {
                    StartDash();
                    return;
                }
            }
        }

        // Disparo circular
        if (canShootArea)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                ShootAreaAttack();
                shootTimer = shootCooldown;
            }
        }

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

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
    }

    private void DashTowardsPlayer()
    {
        if (target == null) return;
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * dashSpeed * Time.deltaTime;
    }

    private void ShootAreaAttack()
    {
        if (bulletPrefab == null || shootPoint == null) return;
        float angleStep = 360f / bulletsPerShot;
        float angle = 0f;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, rot);
            // La bala usará transform.right como dirección inicial
            angle += angleStep;
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
            feedbackManager.ShowFeedback(floatingFeedback, feedbackOrigin, amount.ToString());
        }
        // Debug.Log($"Salud actual del enemigo después de recibir daño: {currentHealth}");
        if (currentHealth <= 0)
        {
            if (deathParticlesPrefab != null)
            {
                Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            }

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
