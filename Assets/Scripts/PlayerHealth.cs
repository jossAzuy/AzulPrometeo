using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Invulnerability Settings")]
    [SerializeField] private float invulnerabilityDuration = 1f;
    [SerializeField] private float flashInterval = 0.1f;
    [SerializeField] private SpriteRenderer playerSprite; // Asigna el SpriteRenderer del jugador en el inspector
    private bool isInvulnerable = false;

    [Header("Audio")]
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AudioClip damageClip;

    [Header("Health Settings")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Slider healthSlider; // Asigna el componente Slider en el inspector

    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOverMenu; // Asigna el menú de Game Over en el inspector

    private int currentLives;

    private void Start()
    {
        currentLives = maxLives;
        UpdateHealthSlider();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;
        currentLives -= damage;
        if (currentLives < 0) currentLives = 0;
        UpdateHealthSlider();
        PlayDamageSound();
        StartCoroutine(InvulnerabilityCoroutine());
        if (currentLives == 0)
        {
            Debug.Log("El jugador ha perdido todas sus vidas.");
            if (gameOverMenu != null)
            {
                gameOverMenu.SetActive(true); // Mostrar el menú de Game Over
            }
            // Aquí puedes agregar lógica adicional de Game Over
        }
    }

    private System.Collections.IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        bool visible = true;
        while (elapsed < invulnerabilityDuration)
        {
            if (playerSprite != null)
                playerSprite.enabled = visible;
            visible = !visible;
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }
        if (playerSprite != null)
            playerSprite.enabled = true;
        isInvulnerable = false;
    }

    private void PlayDamageSound()
    {
        if (soundManager != null && damageClip != null)
        {
            soundManager.PlaySound(damageClip);
        }
    }

    private void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxLives;
            healthSlider.value = currentLives;
        }
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }
}
