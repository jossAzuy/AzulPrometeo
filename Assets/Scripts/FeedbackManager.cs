using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public void ShowFeedback(FloatingFeedbackText feedbackPrefab, Transform origin, string message)
    {
        if (feedbackPrefab == null || canvas == null || origin == null)
        {
            Debug.LogError("FeedbackManager: Parámetros inválidos para mostrar feedback.");
            return;
        }

        // Convertir la posición del origen al espacio del Canvas
        Vector3 worldPosition = origin.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        FloatingFeedbackText feedbackInstance = Instantiate(feedbackPrefab, canvas.transform);
        RectTransform rectTransform = feedbackInstance.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out Vector2 localPosition))
        {
            rectTransform.anchoredPosition = localPosition;
        }

        feedbackInstance.Play(message);
    }

    public void ShowFeedback(FloatingFeedbackImage feedbackPrefab, Transform origin)
    {
        if (feedbackPrefab == null || canvas == null || origin == null)
        {
            Debug.LogError("FeedbackManager: Parámetros inválidos para mostrar feedback de imagen.");
            return;
        }

        // Convertir la posición del origen al espacio del Canvas
        Vector3 worldPosition = origin.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        FloatingFeedbackImage feedbackInstance = Instantiate(feedbackPrefab, canvas.transform);
        RectTransform rectTransform = feedbackInstance.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out Vector2 localPosition))
        {
            rectTransform.anchoredPosition = localPosition;
        }

        feedbackInstance.Play();
    }
}
