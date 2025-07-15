/* using UnityEngine;
using UnityEngine.UI;
using TMPro; // Importar la biblioteca de TextMeshPro

public class SettingsUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameController gameController; // Referencia al GameController
    [SerializeField] private Slider bpmSlider;
    [SerializeField] private TMP_Text bpmValueText; // Cambiado a TMP_Text
    [SerializeField] private Slider perfectPrecisionSlider;
    [SerializeField] private TMP_Text perfectPrecisionValueText; // Cambiado a TMP_Text
    [SerializeField] private Slider goodPrecisionSlider;
    [SerializeField] private TMP_Text goodPrecisionValueText; // Cambiado a TMP_Text
    [SerializeField] private Slider dashSpeedSlider;
    [SerializeField] private TMP_Text dashSpeedValueText; // Cambiado a TMP_Text
    [SerializeField] private Slider boostedDashSpeedSlider;
    [SerializeField] private TMP_Text boostedDashSpeedValueText; // Cambiado a TMP_Text
    [SerializeField] private Slider moveSpeedSlider;
    [SerializeField] private TMP_Text moveSpeedValueText; // Cambiado a TMP_Text

    private void Start()
    {
        // Inicializar los valores de los sliders y textos con los valores actuales del GameController
        bpmSlider.value = gameController.GetBpm(); // Usar el método GetBpm
        bpmValueText.text = gameController.GetBpm().ToString("F0");

        perfectPrecisionSlider.value = gameController.GetPerfectPrecision() * 100f; // Convertir a porcentaje
        perfectPrecisionValueText.text = (gameController.GetPerfectPrecision() * 100f).ToString("F1");

        goodPrecisionSlider.value = gameController.GetGoodPrecision() * 100f; // Convertir a porcentaje
        goodPrecisionValueText.text = (gameController.GetGoodPrecision() * 100f).ToString("F1");

        dashSpeedSlider.value = gameController.GetDashSpeed(); // Usar el método GetDashSpeed
        dashSpeedValueText.text = gameController.GetDashSpeed().ToString("F1");

        boostedDashSpeedSlider.value = gameController.GetBoostedDashSpeed(); // Usar el método GetBoostedDashSpeed
        boostedDashSpeedValueText.text = gameController.GetBoostedDashSpeed().ToString("F1");

        moveSpeedSlider.value = gameController.GetMoveSpeed(); // Usar el método GetMoveSpeed
        moveSpeedValueText.text = gameController.GetMoveSpeed().ToString("F1");
    }

    public void OnBpmSliderChanged(float value)
    {
        gameController.SetBpm(value); // Usar el método SetBpm
        bpmValueText.text = value.ToString("F0");
    }

    public void OnPerfectPrecisionSliderChanged(float value)
    {
        gameController.SetPerfectPrecision(value / 100f); // Convertir de porcentaje a tiempo y usar SetPerfectPrecision
        perfectPrecisionValueText.text = value.ToString("F1");
    }

    public void OnGoodPrecisionSliderChanged(float value)
    {
        gameController.SetGoodPrecision(value / 100f); // Convertir de porcentaje a tiempo y usar SetGoodPrecision
        goodPrecisionValueText.text = value.ToString("F1");
    }

    public void OnDashSpeedSliderChanged(float value)
    {
        gameController.SetDashSpeed(value); // Usar el método SetDashSpeed
        dashSpeedValueText.text = value.ToString("F1");
    }

    public void OnBoostedDashSpeedSliderChanged(float value)
    {
        gameController.SetBoostedDashSpeed(value); // Usar el método SetBoostedDashSpeed
        boostedDashSpeedValueText.text = value.ToString("F1");
    }

    public void OnMoveSpeedSliderChanged(float value)
    {
        gameController.SetMoveSpeed(value); // Usar el método SetMoveSpeed
        moveSpeedValueText.text = value.ToString("F1");
    }
} */