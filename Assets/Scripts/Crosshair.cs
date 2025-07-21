using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair Settings")]
    [SerializeField] private RectTransform crosshairImage; // Asigna la imagen de la mira en el inspector

    void Start()
    {
        // Ocultar el cursor del mouse
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        // Obtener la posición del mouse en la pantalla
        Vector2 mousePosition = Input.mousePosition;

        // Convertir la posición del mouse a coordenadas del mundo del canvas
        if (crosshairImage != null && Camera.main != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            crosshairImage.position = worldPosition;
        }
    }
}