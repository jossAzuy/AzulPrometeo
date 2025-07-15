using UnityEngine;

public class TopDownController : MonoBehaviour
{
    // Variables de velocidad y dash
    public float moveSpeed = 5f; // Velocidad normal de movimiento
    public float dashSpeed = 12f; // Velocidad durante el dash
    public float dashDuration = 0.2f; // Duración del dash en segundos
    public float dashCooldown = 1f; // Tiempo de espera entre dashes
    
    private Rigidbody2D rb;
    private Vector2 moveInput; // Entrada del jugador
    private bool isDashing = false; // Indica si el personaje está en dash
    private float dashTime; // Tiempo en que termina el dash
    private float lastDashTime; // Último momento en que se hizo un dash
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D del objeto
    }

    void Update()
    {
        // Si no está en dash, captura la entrada del jugador
        if (!isDashing)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal"); // Movimiento en eje X
            moveInput.y = Input.GetAxisRaw("Vertical"); // Movimiento en eje Y
            moveInput.Normalize(); // Normaliza la dirección para evitar velocidades más altas en diagonales
        }

        // Si se presiona la tecla de dash (Espacio) y el cooldown ha pasado, se inicia el dash
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            // Mientras dura el dash, mueve al personaje a mayor velocidad
            rb.linearVelocity = moveInput * dashSpeed;
            if (Time.time >= dashTime) // Verifica si terminó el dash
            {
                isDashing = false;
            }
        }
        else
        {
            // Movimiento normal cuando no está en dash
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    void StartDash()
    {
        isDashing = true; // Activa el estado de dash
        dashTime = Time.time + dashDuration; // Define el tiempo en que terminará el dash
        lastDashTime = Time.time; // Guarda el tiempo del último dash
    }
}
