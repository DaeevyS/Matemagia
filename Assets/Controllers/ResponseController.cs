using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ResponseController : MonoBehaviour
{
    [Header("Video Configuration")]
    public VideoPlayer correctVideoPlayer;
    public VideoPlayer incorrectVideoPlayer;
    public RawImage correctVideoImage;
    public RawImage incorrectVideoImage;

    // Botones para marcar la respuesta
    public Button correctButton;
    public Button incorrectButton;

    private void Start()
    {
        // Desactivar las imágenes de video al inicio
        correctVideoImage.gameObject.SetActive(false);
        incorrectVideoImage.gameObject.SetActive(false);

        // Configurar acciones para los botones
        correctButton.onClick.AddListener(() => PlayVideo("correcto"));
        incorrectButton.onClick.AddListener(() => PlayVideo("incorrecto"));

        // Configurar acciones al finalizar cada video
        correctVideoPlayer.loopPointReached += OnVideoEnd;
        incorrectVideoPlayer.loopPointReached += OnVideoEnd;
    }

    private void PlayVideo(string result)
    {
        // Mostrar y reproducir el video adecuado según la respuesta
        if (result == "correcto")
        {
            correctVideoImage.gameObject.SetActive(true);
            correctVideoPlayer.Play();
        }
        else if (result == "incorrecto")
        {
            incorrectVideoImage.gameObject.SetActive(true);
            incorrectVideoPlayer.Play();
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Ocultar ambos videos y regresar a la escena principal (ajusta el nombre de la escena según sea necesario)
        correctVideoImage.gameObject.SetActive(false);
        incorrectVideoImage.gameObject.SetActive(false);
        SceneManager.LoadScene("ruleta");
    }
}


