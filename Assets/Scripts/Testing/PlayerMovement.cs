using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento del jugador
    [SerializeField] private float rotationSpeed = 720f; // Velocidad de rotación del jugador
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("IsEvading", true);
            Debug.Log("Evading");
        }

        MovePlayer();
    }

    private void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Crear un vector de movimiento basado en los ejes de entrada (plano XZ)
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        // Mover al jugador
        if (movement.magnitude > 0.1f)
        {
            transform.position += movement.normalized * moveSpeed * Time.deltaTime;

            // Rotar al jugador hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Actualizar la animación
        bool isWalking = movement.magnitude > 0.1f;
        if (animator != null)
        {
            animator.SetBool("IsWalking", isWalking);
        }
    }
}
