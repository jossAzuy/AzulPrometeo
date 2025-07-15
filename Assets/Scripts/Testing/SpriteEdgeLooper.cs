using UnityEngine;

public class SpriteEdgeLooper : MonoBehaviour
{
    private float spriteWidth; // Ancho del sprite

    void Start()
    {
        // Calcular el ancho del sprite
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Calcular la posición de la cámara
        Camera mainCamera = Camera.main;
        float halfScreenWidth = mainCamera.orthographicSize * mainCamera.aspect;

        // Calcular los límites de la cámara
        float leftLimit = mainCamera.transform.position.x - halfScreenWidth;
        float rightLimit = mainCamera.transform.position.x + halfScreenWidth;

        // Verificar si el sprite está fuera del límite izquierdo de la cámara
        if (transform.position.x + spriteWidth / 2 < leftLimit)
        {
            // Reposicionar el sprite en el extremo derecho de la cámara
            transform.position = new Vector3(rightLimit + spriteWidth / 2, transform.position.y, transform.position.z);
        }
    }
}
