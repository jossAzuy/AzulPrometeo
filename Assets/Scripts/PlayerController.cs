using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private SoundManager soundManager;

    [Header("Movement Settings")]
    [SerializeField] private bool enableMovement = true; // Habilitar o deshabilitar el movimiento
    [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento del jugador

    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab; // Prefab de la bala normal
    [SerializeField] private GameObject perfectBulletPrefab; // Prefab de la bala perfecta
    [SerializeField] private Transform firePoint; // Punto de disparo (asignar en el inspector)
    [SerializeField] private GameController gameController; // Referencia al GameController para el ritmo

    [Header("Weapon Cadence")]
    [SerializeField] private float fireRate = 0.25f; // Tiempo mínimo entre disparos (segundos)
    private float nextFireTime = 0f;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 6; // Munición máxima por recarga
    [SerializeField] private float reloadTime = 1.5f; // Tiempo de recarga en segundos
    private int currentAmmo;
    private bool isReloading = false;

    [Header("Dash Settings")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [Header("Perfect Beat Dash Settings")]
    [SerializeField] private float perfectDashForce = 15f;
    [SerializeField] private float perfectDashDuration = 0.3f;
    [SerializeField] private float perfectDashCooldown = 0.5f;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    private Vector2 dashDirection;
    private Rigidbody2D rb;
    private bool isPerfectDash = false;

    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del jugador

    [Header("Dash Visuals")]
    [SerializeField] private TrailRenderer dashTrail;
    [Header("Dash Feedback")]
    [SerializeField] private FeedbackManager feedbackManager;
    [SerializeField] private FloatingFeedbackText dashFeedbackText;
    [SerializeField] private Transform dashFeedbackOrigin;
    [SerializeField] private FloatingFeedbackImage dashFeedbackImage;

    public event Action OnAmmoChanged;
    public event Action OnReload;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Manejar el Movimiento del Player
        if (enableMovement)
        {
            MovePlayer();
        }

        // Lógica de recarga
        if (isReloading)
            return;

        // Recargar si se presiona R y no está recargando y no está la munición llena
        if ((Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo) || (currentAmmo == 0 && !isReloading))
        {
            StartCoroutine(Reload());
            return;
        }

        // Lógica de disparo conectada al ritmo
        HandleShooting();

        // Dash cooldown
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // Manejar Dash
        if (!isDashing && dashCooldownTimer <= 0f && Input.GetKeyDown(KeyCode.Space))
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            Vector2 inputDir = new Vector2(moveHorizontal, moveVertical);
            if (inputDir.sqrMagnitude > 0.01f)
            {
                dashDirection = inputDir.normalized;
                // Llama a GameController para mostrar la retroalimentación visual de dash
                if (gameController != null)
                {
                    gameController.RegisterRhythmInput(transform);
                }
                // Determina si el dash fue perfecto para la física, pero la retroalimentación visual la maneja GameController
                isPerfectDash = gameController != null && gameController.rhythmSystem != null && gameController.rhythmSystem.IsPerfectBeat();
                StartCoroutine(DashCoroutine());
            }
        }
    }

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Método para mover al jugador
    private void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);

        transform.position += moveSpeed * Time.deltaTime * movement;

        FlipPlayer(moveHorizontal); // Llamar a FlipPlayer para ajustar la dirección del jugador
    }

    private void FlipPlayer(float moveHorizontal)
    {
        if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = false; // Mirar a la derecha
        }
        else if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = true; // Mirar a la izquierda
        }
    }

    // Lógica de disparo del jugador
    private void HandleShooting()
    {
        // Detectar input de disparo (clic izquierdo)
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo <= 0 || isReloading)
            {
                Debug.Log("Sin munición o recargando. No se puede disparar.");
                return; // Salir sin activar la lógica de disparo
            }
            // Llama a GameController para procesar la retroalimentación de ritmo
            if (gameController != null)
            {
                gameController.RegisterRhythmInput(transform);
            }
            // El disparo ocurre igual, pero la retroalimentación visual de ritmo la maneja GameController
            bool isPerfect = gameController != null && gameController.rhythmSystem != null && gameController.rhythmSystem.IsPerfectBeat();
            Shoot(isPerfect);
            currentAmmo--;
            nextFireTime = Time.time + fireRate;
            OnAmmoChanged?.Invoke();
        }
    }

    // Instanciar el proyectil
    private void Shoot(bool isPerfect)
    {
        if (currentAmmo <= 0 || isReloading)
        {
            Debug.Log("Intento de disparo bloqueado: sin balas o recargando.");
            return; // Evitar cualquier lógica de disparo si no hay balas o está recargando
        }

        GameObject prefabToUse = isPerfect ? perfectBulletPrefab : bulletPrefab;
        if (prefabToUse != null && firePoint != null)
        {
            Instantiate(prefabToUse, firePoint.position, firePoint.rotation);
            Debug.Log(isPerfect ? "Disparo PERFECTO" : "Disparo normal");
            if (soundManager != null) soundManager.PlayShoot();
        }
        else
        {
            Debug.LogWarning("Prefab de bala o firePoint no asignados en PlayerController");
        }
    }

    // Corrutina de recarga
    private System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Recargando...");
        if (soundManager != null) soundManager.PlayReload();
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Recarga completa");
        OnReload?.Invoke();
        OnAmmoChanged?.Invoke();
    }

    private System.Collections.IEnumerator DashCoroutine()
    {
        isDashing = true;
        if (dashTrail != null) dashTrail.emitting = true;
        // La retroalimentación visual de dash ahora la maneja GameController
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float force = isPerfectDash ? perfectDashForce : dashForce;
        float duration = isPerfectDash ? perfectDashDuration : dashDuration;
        float cooldown = isPerfectDash ? perfectDashCooldown : dashCooldown;
        rb.linearVelocity = dashDirection * force;
        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;
        dashCooldownTimer = cooldown;
        if (dashTrail != null) dashTrail.emitting = false;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}