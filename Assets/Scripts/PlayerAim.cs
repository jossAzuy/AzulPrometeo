using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("Aim Settings")]
    [SerializeField] private Camera mainCamera; // Cámara principal

    void Update()
    {
        AimTowardsMouse();
    }

    private void AimTowardsMouse()
    {
        // Obtener la posición del ratón en el mundo
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Asegurarse de que la posición Z sea 0

        // Calcular la dirección desde el jugador hacia el ratón
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        // Calcular el ángulo de rotación
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Aplicar la rotación al jugador
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}

