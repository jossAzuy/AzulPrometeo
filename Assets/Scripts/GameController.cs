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

    public RhythmSystem rhythmSystem { get; private set; }

    [SerializeField] private CameraRhythmShake cameraShake; // Arrastra tu cámara con este script en el inspector

    private int lastBeat = -1;
    private int lastPerfectBeat = -1; // Nuevo: último beat perfecto registrado

    // Porcentaje del intervalo para ambos efectos (ajustable desde el inspector)
    [SerializeField, Range(0.1f, 0.9f)]
    private float syncEffectPercent = 0.5f;

    [SerializeField] private Image centerIcon;
    //[SerializeField] private float centerIconShowTime = 0.15f;
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
        rhythmSystem.goodPrecision = goodPrecision;
        audioSource.Play();
        SetBpmAndSyncEffects(bpm);
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
        if (pendingInput && centerIconTimer > 0f && currentBeat != lastPerfectBeat)
        {
            Debug.Log($"[INPUT] Espacio presionado en el ritmo PERFECTO | songPositionInBeats: {songPositionInBeats:F3} | beatPosition: {beatPosition:F3} | currentBeat: {currentBeat}");
            lastPerfectBeat = currentBeat;
            pendingInput = false;

            // Activar texto de retroalimentación en la UI
            if (feedbackText != null)
            {
                Debug.Log("[DEBUG] Activando texto de retroalimentación en la UI.");
                feedbackText.text = "PERFECT!";
                feedbackText.enabled = true;
                StartCoroutine(HideFeedbackTextAfterDelay(0.25f)); // Ocultar después de 1 segundo
            }
            else
            {
                Debug.LogError("[ERROR] feedbackText no está asignado en el inspector.");
            }
        }
        else if (pendingInput && centerIconTimer <= 0f)
        {
            Debug.Log("[DEBUG] Entrada detectada fuera de la ventana de precisión.");
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
            // Mostrar el ícono central y reiniciar temporizador SOLO cuando las flechas llegan al centro
            // Esto ocurre después del tiempo de viaje
            StartCoroutine(ShowCenterIconAfterDelay(travelTime, holdTime));
            lastBeat = currentBeat;
        }

        // Controla la ventana de precisión y el ícono central
        if (centerIcon != null && centerIcon.enabled)
        {
            centerIconTimer -= Time.deltaTime;
            if (centerIconTimer <= 0f)
            {
                centerIcon.enabled = false;
            }
        }
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

    private System.Collections.IEnumerator ShowCenterIconAfterDelay(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        if (centerIcon != null)
        {
            centerIcon.enabled = true;
            centerIconTimer = duration;
        }
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