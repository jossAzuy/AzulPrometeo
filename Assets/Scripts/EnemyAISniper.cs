using UnityEngine;

public class EnemyAISniper : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private FloatingFeedbackText floatingFeedback;
    private int currentHealth;

    [Header("Rythm Settings")]
    [SerializeField] public GameController gameController;

    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private float attackRange = 8f;
    [SerializeField] private float visionRange = 12f;
    [SerializeField] private int damage = 2;
    [SerializeField] public Transform target;

    [Header("Feedback Settings")]
    [SerializeField] private FeedbackManager feedbackManager;
    [SerializeField] private Transform feedbackOrigin;

    [Header("Drop Settings")]
    public bool dropCoinsOnDeath = true;
    public GameObject coinPrefab;

    [Header("Sniper Settings")]
    [SerializeField] private float aimTime = 1.5f;
    [SerializeField] private float cooldownTime = 2.5f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    private float aimTimer = 0f;
    private float cooldownTimer = 0f;
    private bool isAiming = false;
    private GameObject currentLaser;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (gameController == null || gameController.rhythmSystem == null || !gameController.rhythmSystem.IsPerfectBeat())
            return;
        if (target == null)
            return;
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer <= visionRange)
        {
            if (distanceToPlayer > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                SniperAttack();
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    private void SniperAttack()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }
        if (!isAiming)
        {
            isAiming = true;
            aimTimer = aimTime;
            if (laserPrefab != null && firePoint != null)
            {
                currentLaser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity, firePoint);
                currentLaser.transform.right = (target.position - firePoint.position).normalized;
            }
        }
        else
        {
            aimTimer -= Time.deltaTime;
            if (currentLaser != null && firePoint != null)
            {
                currentLaser.transform.right = (target.position - firePoint.position).normalized;
            }
            if (aimTimer <= 0f)
            {
                FireBullet();
                isAiming = false;
                cooldownTimer = cooldownTime;
                if (currentLaser != null)
                {
                    Destroy(currentLaser);
                }
            }
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector3 dir = (target.position - firePoint.position).normalized;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * 10f;
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.damage = damage;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
                Destroy(collision.gameObject);
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
