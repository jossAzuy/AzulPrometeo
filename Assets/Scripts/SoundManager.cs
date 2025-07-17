using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reloadClip;

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

    public void PlaySound(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
            sfxSource.PlayOneShot(clip);
    }
}
