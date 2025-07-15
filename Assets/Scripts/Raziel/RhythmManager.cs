using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour
{
    public float bpm = 120f; //Velocidad de la música en beats por minuto
    public Image marker; //Referencia al marcador visual
    public AudioSource audioSource; //Referencia de la música
    private float interval; //Tiempo entre beats

    void Start()
    {
        interval = 60f / bpm; //Calcula la duración de un beat
        audioSource.Play(); //Inicia la música
    }

    void Update()
    {
        float songTime = audioSource.time; //Tiempo exacto de la canción
        float beatPosition = songTime % interval; //En qué parte del beat estamos

        if (beatPosition < 0.1f) //Si estamos cerca del inicio de un beat
        {
            PulseMarker(); //Animar el marcador
        }

        if (Input.GetKeyDown(KeyCode.Space)) //Si el jugador presiona la tecla space, cambien esto si no se va a usar esta tecla
        {
            CheckTiming(beatPosition);
        }
    }

    void PulseMarker()
    {
        marker.transform.localScale = Vector3.one * 1.2f; //Agranda el marcador
        Invoke("ResetMarker", 0.1f); //Restaurar su tamaño después de 0.1s
    }

    void ResetMarker()
    {
        marker.transform.localScale = Vector3.one; //Restaura el tamaño
    }

    void CheckTiming(float beatPosition)
    {
        float precision = Mathf.Abs(beatPosition - interval / 2); //Diferencia con el centro del beat

        if (precision < 0.05f) //Puntuacion Perfecta
        {
            Debug.Log("¡Perfecto!");
            marker.color = Color.green;
        }
        else if (precision < 0.1f) //Bien
        {
            Debug.Log("¡Bien!");
            marker.color = Color.yellow;
        }
        else //No le atino
        {
            Debug.Log("Fuera de tiempo");
            marker.color = Color.red;
        }

        Invoke("ResetMarkerColor", 0.2f); //Restaura el color después de 0.2s
    }

    void ResetMarkerColor()
    {
        marker.color = Color.white; //Restaurar el color original
    }
}