using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private bool enableMovement = true; // Habilitar o deshabilitar el movimiento
    [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento del jugador


    void Update()
    {
        // Manejar el Movimiento del Player
        if (enableMovement)
        {
            MovePlayer();
        }

    }

    // Método para mover al jugador
    private void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);

        // Normalizar solo si hay entrada (magnitud > 0)
        /*  if (movement.magnitude > 0f) */
        //movement = movement.normalized;

        transform.position += moveSpeed * Time.deltaTime * movement;
        // Debug.Log("Player movement: " + movement);
    }
}