using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ESP32ButtonListener : MonoBehaviour
{
    public VideoPlayer correctoVideoPlayer;
    public VideoPlayer incorrectoVideoPlayer;
    public RawImage videoImageC;
    public RawImage videoImageI;

    private string receivedData = ""; // Almacena los caracteres recibidos

    private void Start()
    {
        correctoVideoPlayer.loopPointReached += OnVideoEnd;
        incorrectoVideoPlayer.loopPointReached += OnVideoEnd;

        videoImageC.gameObject.SetActive(false);
        videoImageI.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Verifica si Unity está recibiendo caracteres del "teclado" Bluetooth
        if (!string.IsNullOrEmpty(Input.inputString))
        {
            receivedData += Input.inputString; // Agrega caracteres a la cadena recibida
            Debug.Log("Cadena parcial recibida: " + receivedData);
            
            // Comprueba si contiene "CORRECTO" o "INCORRECTO"
            if (receivedData.Contains("CORRECTO"))
            {
                Debug.Log("Recibido: CORRECTO");
                PlayCorrectoVideo();
                receivedData = ""; // Reinicia la cadena recibida después de procesar
            }
            else if (receivedData.Contains("INCORRECTO"))
            {
                Debug.Log("Recibido: INCORRECTO");
                PlayIncorrectoVideo();
                receivedData = ""; // Reinicia la cadena recibida después de procesar
            }
            // Opcional: Limitar el tamaño de receivedData para evitar acumulación de caracteres no deseados
            else if (receivedData.Length > 50)
            {
                receivedData = ""; // Borra el buffer si se vuelve muy largo
            }
        }
    }

    private void PlayCorrectoVideo()
    {
        videoImageC.gameObject.SetActive(true);
        incorrectoVideoPlayer.Stop();
        correctoVideoPlayer.Play();
    }

    private void PlayIncorrectoVideo()
    {
        videoImageI.gameObject.SetActive(true);
        correctoVideoPlayer.Stop();
        incorrectoVideoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        videoImageC.gameObject.SetActive(false);
        videoImageI.gameObject.SetActive(false);
        SceneManager.LoadScene("ruleta");
    }
}
