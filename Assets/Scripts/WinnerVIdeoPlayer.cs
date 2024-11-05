using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; // El VideoPlayer que reproducirá el video del ganador
    public string mainMenuSceneName = "Menu"; // Nombre de la escena del menú principal

    void Start()
    {
        // Asegúrate de que el RawImage se muestra y el video comienza a reproducirse
        videoPlayer.loopPointReached += OnVideoEnd; // Registrar evento para el final del video
        videoPlayer.Play(); // Comenzar a reproducir el video
    }

    // Función que se llama cuando el video termina
    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(mainMenuSceneName); // Cargar la escena del menú principal
    }
}
