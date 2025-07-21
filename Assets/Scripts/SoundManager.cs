using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // Singleton Instance

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;
    [SerializeField] private AudioClip enemyDeathClip; // Clip de sonido para la muerte del enemigo

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Asegurar que solo haya una instancia
        }
    }

    public void PlayShoot()
    {
        if (sfxSource != null && shootClip != null)
            sfxSource.PlayOneShot(shootClip);
    }

    public void PlayReload()
    {
        if (sfxSource != null && reloadClip != null)
            sfxSource.PlayOneShot(reloadClip);
    }

    public void PlayEnemyDeath()
    {
        if (sfxSource != null && enemyDeathClip != null)
            sfxSource.PlayOneShot(enemyDeathClip);
    }

    public void PlaySound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip);
    }
}
