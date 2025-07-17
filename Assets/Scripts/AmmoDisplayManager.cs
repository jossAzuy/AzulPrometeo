using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplayManager : MonoBehaviour
{
    [Header("Ammo Display Settings")]
    [SerializeField] private Image[] ammoSprites; // Conjunto de sprites que representan las balas
    [SerializeField] private PlayerController playerController; // Referencia al PlayerController
    [SerializeField] private TMPro.TMP_Text ammoText; // Texto para mostrar la cantidad de balas

    private void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController no asignado en AmmoDisplayManager.");
            return;
        }

        UpdateAmmoDisplay();
    }

    private void OnEnable()
    {
        playerController.OnAmmoChanged += UpdateAmmoDisplay;
        playerController.OnReload += ResetAmmoDisplay;
    }

    private void OnDisable()
    {
        playerController.OnAmmoChanged -= UpdateAmmoDisplay;
        playerController.OnReload -= ResetAmmoDisplay;
    }

    private void UpdateAmmoDisplay()
    {
        int currentAmmo = playerController.GetCurrentAmmo();
        for (int i = 0; i < ammoSprites.Length; i++)
        {
            ammoSprites[i].enabled = i < currentAmmo;
        }
        if (ammoText != null)
            ammoText.text = "x" + currentAmmo.ToString("D2");
    }

    private void ResetAmmoDisplay()
    {
        foreach (Image sprite in ammoSprites)
        {
            sprite.enabled = true;
        }
        if (ammoText != null)
            ammoText.text = "x" + playerController.GetCurrentAmmo().ToString("D2");
    }
}
