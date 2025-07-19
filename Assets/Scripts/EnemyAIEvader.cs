using UnityEngine;

public class EnemyAIEvader : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private FloatingFeedbackText floatingFeedback;
    private int currentHealth;

    [Header("Rythm Settings")]
    [SerializeField] public GameController gameController;

    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] public Transform target;

    [Header("Feedback Settings")]
    [SerializeField] private FeedbackManager feedbackManager;
    [SerializeField] private Transform feedbackOrigin;

    [Header("Drop Settings")]
    public bool dropCoinsOnDeath = false;
    public GameObject coinPrefab;

    [Header("Enemy Variant")]
    public bool alwaysFollowPlayer = false;

    [Header("Evade Settings")]
    [SerializeField] private float evadeChance = 0.5f; // Probabilidad de esquivar (0-1)
    [SerializeField] private float evadeDistance = 1.5f; // Distancia lateral de esquiva



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

        // Copia la lógica de EnemyAI pero usa FollowPlayerWithEvade en vez de FollowPlayer
        if (alwaysFollowPlayer)
        {
            if (distanceToPlayer > attackRange)
            {
                FollowPlayerWithEvade();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            if (distanceToPlayer <= visionRange)
            {
                if (distanceToPlayer > attackRange)
                {
                    FollowPlayerWithEvade();
                }
                else
                {
                    Attack();
                }
            }
            // Si quieres patrulla, aquí podrías agregar lógica de patrulla opcional
        }
    }

    // Igual que FollowPlayer pero con esquiva
    private void FollowPlayerWithEvade()
    {
        Vector3 toPlayer = target.position - transform.position;
        if (toPlayer.sqrMagnitude < 0.0001f)
        {
            toPlayer = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized;
        }
        else
        {
            toPlayer = toPlayer.normalized;
        }
        Vector3 evadeDir = Vector3.zero;
        if (Random.value < evadeChance)
        {
            Vector3 perp = Vector3.Cross(toPlayer, Vector3.forward).normalized;
            if (Random.value < 0.5f)
                perp = -perp;
            evadeDir = perp * evadeDistance;
        }
        Vector3 moveDir = (toPlayer + evadeDir).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void Attack()
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
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
