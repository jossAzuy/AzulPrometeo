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

    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del jugador

    public event Action OnAmmoChanged;
    public event Action OnReload;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}