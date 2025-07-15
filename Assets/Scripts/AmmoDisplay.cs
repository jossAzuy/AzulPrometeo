using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoDisplay : MonoBehaviour
{
    /* [Header("Ammo Settings")]
    [SerializeField] private Image[] ammoImages; // Imágenes que representan las balas
    [SerializeField] private PlayerController playerController; // Referencia al script del jugador

    private bool isReloading = false; // Indicador de si se está recargando

    void Start()
    {
        // Inicializar el estado de las imágenes de balas
        UpdateAmmoDisplay(playerController.GetMaxShotsBeforeReload());
    }

    void Update()
    {
        if (!isReloading)
        {
            // Actualizar el estado de las imágenes de balas
            UpdateAmmoDisplay(playerController.GetRemainingShots());
        }
    }

    // Método para actualizar el estado de las imágenes de balas
    private void UpdateAmmoDisplay(int remainingShots)
    {
        for (int i = 0; i < ammoImages.Length; i++)
        {
            if (i < remainingShots)
            {
                ammoImages[i].enabled = true; // Activar la imagen si hay balas disponibles
            }
            else
            {
                ammoImages[i].enabled = false; // Desactivar la imagen si no hay balas disponibles
                Debug.Log(ammoImages[i]);
            }
        }
    } */
}