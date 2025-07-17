using UnityEngine;
using UnityEngine.UI;

public class FloatingFeedbackImage : MonoBehaviour
{
    public float floatDistance = 50f;
    public float duration = 0.5f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public bool useInitialPosition = true; // Nueva opción para controlar el comportamiento de la posición

    private Image image;
    private Vector3 startPos;
    private Color startColor;
    private float timer;
    private bool isPlaying = false;

    void Awake()
    {
        image = GetComponent<Image>();
        startPos = transform.localPosition;
        startColor = image.color;
    }

    void Start()
    {
        // Eliminar la desactivación inicial de la imagen
    }

    public void Play()
    {
        image.enabled = true;
        timer = 0f;
        isPlaying = true;
        image.color = startColor;
        if (useInitialPosition)
        {
            transform.localPosition = startPos;
        }
    }

    void Update()
    {
        if (!isPlaying) return;
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        if (useInitialPosition)
        {
            // Mover hacia arriba
            transform.localPosition = startPos + Vector3.up * floatDistance * t;
        }
        // Fade out
        Color c = image.color;
        c.a = fadeCurve.Evaluate(t);
        image.color = c;
        if (t >= 1f)
        {
            isPlaying = false;
            image.enabled = false;
            Destroy(gameObject); // Destruir el objeto después de la animación
        }
    }

    public void ResetAndActivate()
    {
        image.enabled = true; // Reactivar la imagen
        timer = 0f; // Reiniciar el temporizador
        isPlaying = true; // Reiniciar el estado de reproducción
        image.color = startColor; // Restaurar el color inicial
        if (useInitialPosition)
        {
            transform.localPosition = startPos; // Restaurar la posición inicial
        }
    }
}
