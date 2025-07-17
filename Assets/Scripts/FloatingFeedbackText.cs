using UnityEngine;
using TMPro;

public class FloatingFeedbackText : MonoBehaviour
{
    public float floatDistance = 50f;
    public float duration = 0.5f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public bool useInitialPosition = true; // Nueva opción para controlar el comportamiento de la posición

    private TMP_Text tmpText;
    private Vector3 startPos;
    private Color startColor;
    private float timer;
    private bool isPlaying = false;

    void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        startPos = transform.localPosition;
        startColor = tmpText.color;
    }

    void Start()
    {
        // Eliminar la desactivación inicial del texto
        // tmpText.enabled = false;
    }

    public void Play(string message)
    {
        tmpText.text = message;
        tmpText.enabled = true;
        timer = 0f;
        isPlaying = true;
        tmpText.color = startColor;
        if (useInitialPosition)
        {
            transform.localPosition = startPos;
        }
    }

    public void ResetAndActivate(string message)
    {
        tmpText.text = message; // Actualizar el texto
        tmpText.enabled = true; // Reactivar el texto
        timer = 0f; // Reiniciar el temporizador
        isPlaying = true; // Reiniciar el estado de reproducción
        tmpText.color = startColor; // Restaurar el color inicial
        if (useInitialPosition)
        {
            transform.localPosition = startPos; // Restaurar la posición inicial
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
        Color c = tmpText.color;
        c.a = fadeCurve.Evaluate(t);
        tmpText.color = c;
        if (t >= 1f)
        {
            isPlaying = false;
            tmpText.enabled = false;
            Destroy(gameObject); // Destruir el objeto después de la animación
        }
    }
}

// Este script ahora será manejado por FeedbackManager y no se usará directamente.
