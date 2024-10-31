using UnityEngine;
using UnityEngine.UI;          // Para trabajar con el componente UI Text
using UnityEngine.Video;       // Para el reproductor de video
using UnityEngine.SceneManagement; // Para cambiar de escena

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 50f;   // Tiempo inicial (50 segundos)
    public Text countdownText;          // Componente de texto UI para mostrar el tiempo
    public VideoPlayer videoPlayer;     // VideoPlayer para reproducir el video
    public RawImage videoImage;         // RawImage para mostrar el video
    public string nextSceneName;        // Nombre de la siguiente escena

    private float currentTime;

    void Start()
    {
        // Inicializar el temporizador
        currentTime = countdownTime;
        UpdateTimerText();

        // Configurar el video para ocultarse al inicio
        videoImage.gameObject.SetActive(false);
        videoPlayer.loopPointReached += OnVideoEnd; // Registrar evento para el final del video
    }

    void Update()
    {
        // Si el tiempo es mayor a 0, reducir el tiempo con el paso de los frames
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Reducir el tiempo cada frame
            UpdateTimerText();             // Actualizar el texto en el UI
        }
        else
        {
            // Asegurarse de que el tiempo no baje de 0
            currentTime = 0;
            UpdateTimerText();
            
            // Reproducir el video cuando el temporizador llega a 0
            if (!videoPlayer.isPlaying)   // Verificar que el video no esté ya reproduciéndose
            {
                PlayVideo();
            }
        }
    }

    // Actualizar el tiempo mostrado en el texto de UI
    void UpdateTimerText()
    {
        countdownText.text = Mathf.Ceil(currentTime).ToString(); // Mostrar solo números enteros
    }

    // Función para reproducir el video
    void PlayVideo()
    {
        videoImage.gameObject.SetActive(true); // Mostrar el RawImage donde se verá el video
        videoPlayer.Play();                    // Reproducir el video
    }

    // Evento que se activa al finalizar el video
    void OnVideoEnd(VideoPlayer vp)
    {
        videoImage.gameObject.SetActive(false); // Ocultar el RawImage cuando el video termina
        SceneManager.LoadScene("ruleta");  // Cambiar a la siguiente escena
    }
}
