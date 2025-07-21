using UnityEngine;
using UnityEngine.UI;

public class RhythmArrowMover : MonoBehaviour
{
    [Header("Arrow Visuals")]
    [SerializeField] private RectTransform leftArrow;
    [SerializeField] private RectTransform rightArrow;
    [SerializeField] private float arrowMoveDistance = 200f;
    private float travelTime = 0.1f; // Tiempo de viaje hasta el centro
    private float holdTime = 0.1f;   // Tiempo de espera en el centro (ventana perfecta)
    [SerializeField] private float arrowGap = 40f;


    private float arrowProgress = 1f;
    private bool arrowActive = false;
    private float arrowTimer = 0f;
    private Vector2 leftArrowStartPos;
    private Vector2 rightArrowStartPos;

    void Start()
    {
        if (leftArrow != null)
            leftArrowStartPos = leftArrow.anchoredPosition;
        if (rightArrow != null)
            rightArrowStartPos = rightArrow.anchoredPosition;
    }

    void Update()
    {
        // Mover flechas visuales
        if (arrowActive)
        {
            arrowTimer -= Time.deltaTime;
            float t;
            if (arrowTimer > holdTime)
            {
                // Animación de viaje hacia el centro
                float travelProgress = 1f - ((arrowTimer - holdTime) / travelTime);
                t = Mathf.Clamp01(travelProgress);
            }
            else if (arrowTimer > 0f)
            {
                // Flechas permanecen en el centro junto al icono central
                t = 1f;
            }
            else
            {
                arrowActive = false;
                t = 0f;
            }

            if (leftArrow != null)
            {
                Vector2 leftTarget = leftArrowStartPos + new Vector2(arrowMoveDistance - arrowGap / 2f, 0);
                leftArrow.anchoredPosition = Vector2.Lerp(leftArrowStartPos, leftTarget, t);
            }
            if (rightArrow != null)
            {
                Vector2 rightTarget = rightArrowStartPos + new Vector2(-arrowMoveDistance + arrowGap / 2f, 0);
                rightArrow.anchoredPosition = Vector2.Lerp(rightArrowStartPos, rightTarget, t);
            }
        }
    }

    // Método público para activar la animación desde GameController
    // Permite pasar el tiempo de viaje y el tiempo de espera como parámetros
    public void TriggerArrowMove(float travel, float hold)
    {
        travelTime = travel;
        holdTime = hold;
        arrowTimer = travel + hold;
        arrowActive = true;
    }
}
