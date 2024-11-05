using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoMenuIntro1 : MonoBehaviour
{
    public VideoPlayer videoPlayer;       // Asigna el VideoPlayer desde el inspector
    public GameObject overlayObject;      // Asigna el objeto que se mostrará durante el video (por ejemplo, un panel)
    public GameObject additionalObject;   // El cuarto objeto que se activará durante el video
    public Button playButton;             // Asigna el botón desde el inspector

    private void Start()
    {
        // Asegúrate de que el overlay, el objeto adicional y el video player estén desactivados al inicio
        overlayObject.SetActive(false);
        additionalObject.SetActive(false);
        videoPlayer.gameObject.SetActive(false);

        // Suscribir el evento del botón para llamar a la función PlayVideo
        playButton.onClick.AddListener(PlayVideo);
        
        // Suscribir el evento de finalización del video para ocultar los objetos y el video player cuando termine
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void PlayVideo()
    {
        // Activar el VideoPlayer, el overlay y el objeto adicional
        videoPlayer.gameObject.SetActive(true);
        overlayObject.SetActive(true);
        additionalObject.SetActive(true);

        // Iniciar la reproducción del video desde el inicio
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Detener el video y desactivar el VideoPlayer, el overlay y el objeto adicional cuando el video termina
        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
        overlayObject.SetActive(false);
        additionalObject.SetActive(false);
    }

    private void OnDestroy()
    {
        // Desuscribir el evento para evitar referencias nulas
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}