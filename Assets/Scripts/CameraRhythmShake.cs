using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Cambia a Universal para URP

public class CameraRhythmShake : MonoBehaviour
{
    private Vector3 originalPos;
    private float targetOrthoSize;
    private Camera cam;

    [Header("Bump Effect")]
    [SerializeField] private bool enableBump = false;
    [SerializeField] private float bumpDistance = 0.2f;
    [SerializeField] private float bumpDuration = 0.2f; // Duración del bump en segundos (se ajusta con el BPM)
    [SerializeField] private Vector3 bumpDirection = new Vector3(0, 1, 0);
    private Vector3 bumpTarget;
    private Vector3 bumpStart;
    private bool isBumping = false;
    private float bumpProgress = 0f;

    [Header("Vignette Effect")]
    [SerializeField] private bool enableVignette = false;
    [SerializeField] private Volume globalVolume; // Asigna tu Global Volume desde el inspector
    private Vignette vignette;
    [SerializeField] private float vignetteIntensity = 0.4f;
    private float targetVignette = 0f;
    private float vignetteFadeDuration = 0.2f;

    private float shakeTimer = 0f;

    void Awake()
    {
        originalPos = transform.localPosition;
        cam = GetComponent<Camera>();
        if (cam != null)
            targetOrthoSize = cam.orthographicSize;
        bumpTarget = originalPos;

        // URP: Obtén el Vignette del Volume Profile
        if (enableVignette && globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out vignette);
            if (vignette != null)
                vignette.intensity.value = 0f;
        }
    }

    void Update()
    {

        // Bump Effect (Interpolación por duración, sincronizado con BPM)
        if (enableBump && isBumping)
        {
            bumpProgress += Time.deltaTime / bumpDuration;
            transform.localPosition = Vector3.Lerp(bumpStart, bumpTarget, bumpProgress);
            if (bumpProgress >= 1f)
            {
                transform.localPosition = originalPos;
                isBumping = false;
            }
        }

        // Vignette/Post-Process Effect (URP)
        if (enableVignette && vignette != null)
        {
            vignette.intensity.value = Mathf.MoveTowards(
                vignette.intensity.value,
                targetVignette,
                Time.deltaTime * (vignetteIntensity / vignetteFadeDuration)
            );
            if (Mathf.Abs(vignette.intensity.value - targetVignette) < 0.01f)
                targetVignette = 0f;
        }
    }

    public void TriggerBump()
    {
        if (enableBump && !isBumping) // Solo activa si no está en bump
        {
            bumpStart = originalPos + bumpDirection.normalized * bumpDistance;
            bumpTarget = originalPos;
            bumpProgress = 0f;
            isBumping = true;
            transform.localPosition = bumpStart;
        }
    }

    public void TriggerVignette()
    {
        if (enableVignette && vignette != null)
        {
            targetVignette = vignetteIntensity;
        }
    }

    public void SetVignetteFadeDuration(float duration)
    {
        vignetteFadeDuration = duration;
    }

    // Recibe la duración del bump desde GameController sincronizado con RhythmSystem
    public void SetBumpDuration(float duration)
    {
        bumpDuration = duration;
    }
}