using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Velocidad de la bala

    void Update()
    {
        // Mover la bala hacia adelante
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        // Destruir la bala cuando salga de la pantalla
        Destroy(gameObject);
    }
}