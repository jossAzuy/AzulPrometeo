using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Rhythm Settings")]
    [SerializeField] private float bpm = 88f; // Velocidad de la música en beats por minuto
    //[SerializeField] private Image marker; // Referencia al marcador visual (barra circular)
    [SerializeField] private AudioSource audioSource; // Referencia de la música
    [SerializeField] private float perfectPrecision = 0.05f; // Margen de precisión para movimientos perfectos
    [SerializeField] private float goodPrecision = 0.1f; // Margen de precisión para movimientos buenos

    [Header("Audio Feedback")]
    [SerializeField] private AudioClip beatClip; // Sonido para el beat
    [SerializeField] private AudioSource sfxSource; // Fuente de audio para efectos (diferente del audioSource de la música)

    [Header("UI Feedback")]
    [SerializeField] private TMPro.TMP_Text feedbackText; // Texto de retroalimentación en la UI
    [SerializeField] private FloatingFeedbackText floatingFeedback; // Script para animar el feedback

    [Header("Feedback Manager")]
    [SerializeField] private FeedbackManager feedbackManager; // Asigna el FeedbackManager en el inspector

    [Header("Image Feedback")]
    [SerializeField] private FloatingFeedbackImage perfectFeedbackImage; // Imagen para aciertos perfectos
    [SerializeField] private FloatingFeedbackImage missFeedbackImage; // Imagen para errores

    public RhythmSystem rhythmSystem { get; private set; }

    [SerializeField] private CameraRhythmShake cameraShake; // Arrastra tu cámara con este script en el inspector

    private int lastBeat = -1;
    private int lastPerfectBeat = -1; // Nuevo: último beat perfecto registrado

    // Porcentaje del intervalo para ambos efectos (ajustable desde el inspector)
    [SerializeField, Range(0.1f, 0.9f)]
    private float syncEffectPercent = 0.5f;

    [SerializeField] private Image centerIcon;
    [SerializeField] private Vector3 centerIconNormalScale = Vector3.one;
    [SerializeField] private Vector3 centerIconBeatScale = new Vector3(1.3f, 1.3f, 1f);
    private Coroutine centerIconScaleCoroutine;
    private float centerIconTimer = 0f;

    private bool isPerfectBeat = false;
    // Buffer para entrada de ritmo
    private bool pendingInput = false;

    // Agrega la referencia al script RhythmArrowMover
    [SerializeField] private RhythmArrowMover arrowMover;

    void Start()
    {
        rhythmSystem = new RhythmSystem(bpm);
        rhythmSystem.perfectPrecision = perfectPrecision;
        //rhythmSystem.goodPrecision = goodPrecision;
        audioSource.Play();
        SetBpmAndSyncEffects(bpm);
        if (centerIcon != null)
        {
            centerIcon.enabled = true;
            centerIcon.rectTransform.localScale = centerIconNormalScale;
        }
    }

    void Update()
    {
        // Detecta si el jugador presionó la tecla
        if (Input.GetMouseButtonDown(0))
            pendingInput = true;

        HandleRhythm();
    }

    private void HandleRhythm()
    {
        // Actualizar ritmo
        rhythmSystem.UpdateRhythm();
        float songPositionInBeats = rhythmSystem.songPositionInBeats;
        float beatPosition = rhythmSystem.beatPosition;
        int currentBeat = rhythmSystem.GetCurrentBeat();

        // Elimina la comprobación directa de IsPerfectBeat para la ventana
        isPerfectBeat = false;


        // Comprobación de acierto en el ritmo perfecto sincronizada (solo una vez por beat)
        if (pendingInput && currentBeat != lastPerfectBeat)
        {
            if (rhythmSystem != null && rhythmSystem.IsPerfectBeat())
            {
                Debug.Log($"[INPUT] Espacio presionado en el ritmo PERFECTO | songPositionInBeats: {songPositionInBeats:F3} | beatPosition: {beatPosition:F3} | currentBeat: {currentBeat}");
                lastPerfectBeat = currentBeat;
                // Usar FeedbackManager para mostrar retroalimentación
                if (feedbackManager != null && floatingFeedback != null)
                {
                    feedbackManager.ShowFeedback(floatingFeedback, transform, "PERFECT!");
                }

                if (feedbackManager != null && perfectFeedbackImage != null)
                {
                    feedbackManager.ShowFeedback(perfectFeedbackImage, transform);
                }
            }
            else
            {
                Debug.Log("[DEBUG] Entrada detectada fuera de la ventana de precisión.");
                if (feedbackManager != null && floatingFeedback != null)
                {
                    feedbackManager.ShowFeedback(floatingFeedback, transform, "MISS!");
                }

                if (feedbackManager != null && missFeedbackImage != null)
                {
                    feedbackManager.ShowFeedback(missFeedbackImage, transform);
                }
            }
            pendingInput = false;
        }

        // Sincroniza efectos y ventana de precisión cuando las flechas llegan al centro
        if (currentBeat != lastBeat)
        {
            float holdTime = rhythmSystem.interval * perfectPrecision; // ventana perfecta
            float travelTime = rhythmSystem.interval - holdTime; // tiempo de viaje antes de la ventana

            // Ajustar duración y sincronización de efectos
            float effectDuration = rhythmSystem.interval * syncEffectPercent;
            cameraShake?.SetVignetteFadeDuration(effectDuration);
            cameraShake?.SetBumpDuration(effectDuration);

            StartCoroutine(AnimateVignetteEffect(travelTime, effectDuration));

            // Ejecutar efecto de bump directamente
            cameraShake?.TriggerBump();

            // Sincroniza la duración de la animación de las flechas con la ventana perfecta
            arrowMover?.TriggerArrowMove(travelTime, holdTime);
            // Retroalimentación sonora sincronizada con el beat
            if (sfxSource != null && beatClip != null)
            {
                sfxSource.PlayOneShot(beatClip);
            }
            // Animar el icono central (siempre visible)
            if (centerIcon != null)
            {
                if (centerIconScaleCoroutine != null)
                    StopCoroutine(centerIconScaleCoroutine);
                centerIconScaleCoroutine = StartCoroutine(AnimateCenterIconScale(holdTime));
            }
            lastBeat = currentBeat;
        }

        // El icono central siempre está visible, no se desactiva
    }

    // Métodos de acceso para Rhythm Settings
    public float GetBpm() => bpm;
    private void SetBpmAndSyncEffects(float value)
    {
        bpm = value;
        if (rhythmSystem != null)
        {
            rhythmSystem.bpm = bpm;
            rhythmSystem.interval = 60f / bpm;
        }
        float effectDuration = (rhythmSystem != null ? rhythmSystem.interval : 60f / bpm) * syncEffectPercent;
        cameraShake.SetVignetteFadeDuration(effectDuration);
        cameraShake.SetBumpDuration(effectDuration);
    }

    public float GetPerfectPrecision() => perfectPrecision;
    public void SetPerfectPrecision(float value) => perfectPrecision = value;

    public float GetGoodPrecision() => goodPrecision;
    public void SetGoodPrecision(float value) => goodPrecision = value;

    // Corrutina para animar el escalado del icono central
    private System.Collections.IEnumerator AnimateCenterIconScale(float holdTime)
    {
        // Escalar rápidamente al tamaño de beat
        float t = 0f;
        float scaleUpTime = 0.08f;
        Vector3 startScale = centerIconNormalScale;
        Vector3 endScale = centerIconBeatScale;
        while (t < scaleUpTime)
        {
            t += Time.deltaTime;
            centerIcon.rectTransform.localScale = Vector3.Lerp(startScale, endScale, t / scaleUpTime);
            yield return null;
        }
        centerIcon.rectTransform.localScale = endScale;

        // Mantener el tamaño durante la ventana perfecta
        yield return new WaitForSeconds(holdTime);

        // Volver suavemente al tamaño original
        t = 0f;
        float scaleDownTime = 0.15f;
        while (t < scaleDownTime)
        {
            t += Time.deltaTime;
            centerIcon.rectTransform.localScale = Vector3.Lerp(endScale, startScale, t / scaleDownTime);
            yield return null;
        }
        centerIcon.rectTransform.localScale = startScale;
    }

    private System.Collections.IEnumerator HideFeedbackTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (feedbackText != null)
        {
            feedbackText.enabled = false;
        }
    }

    private System.Collections.IEnumerator AnimateVignetteEffect(float travelTime, float effectDuration)
    {
        yield return new WaitForSeconds(travelTime);
        if (cameraShake != null)
        {
            cameraShake.TriggerVignette();
            Debug.Log("[DEBUG] Viñeta alcanzando su punto máximo.");
            yield return new WaitForSeconds(effectDuration);
            cameraShake.TriggerVignette(); // Regresar la viñeta
            Debug.Log("[DEBUG] Viñeta regresando después del beat.");
        }
    }
}