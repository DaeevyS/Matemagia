using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class AbacusGameController : MonoBehaviour
{
    public AbacusLineController unitsLine;
    public AbacusLineController tensLine;
    public AbacusLineController hundredsLine;
    public AbacusLineController uMilLine;
    public AbacusLineController dMilLine;
    public Text targetNumberText;
    public Text currentValueText;
    public GameObject unidadDeMil;
    public GameObject decenaDeMil;
    private int targetNumber;

    public VideoPlayer videoPlayer;
    public RawImage videoImage;  // Referencia al RawImage donde se mostrará el video
    private GameManager gameManager; // Referencia al GameManager

    void Start()
    {
        gameManager = GameManager.Instance;
        GenerateNewNumber();
        videoPlayer.loopPointReached += OnVideoEnd;
        videoImage.gameObject.SetActive(false);  // Ocultar el RawImage al inicio
    }

    void Update()
    {
        int currentValue = unitsLine.CalculateValue() + 
                           tensLine.CalculateValue() * 10 + 
                           hundredsLine.CalculateValue() * 100 +
                           uMilLine.CalculateValue() * 1000 +
                           dMilLine.CalculateValue() * 10000;
        currentValueText.text = "Current Value: " + currentValue;

        if (currentValue == targetNumber && !videoPlayer.isPlaying)
        {
            Debug.Log("¡Correcto! Reproduciendo video...");
            PlayVideo();

            // Determinar la dificultad y asignar puntos según el nombre de la escena
            int points = GetPointsForCurrentScene();
            if (gameManager != null && points > 0)
            {
                gameManager.UpdateScore(points);
            }

        }
    }
    int GetPointsForCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Contains("Facil"))
            return 1;
        else if (sceneName.Contains("Medio"))
            return 2;
        else if (sceneName.Contains("Dificil"))
            return 3;
        
        return 0; // En caso de que no coincida con ninguna dificultad
    }
    void GenerateNewNumber()
    {
        if (unidadDeMil.activeSelf && !decenaDeMil.activeSelf)
        {
            targetNumber = Random.Range(1001, 9999);
        }
        else if (unidadDeMil.activeSelf && decenaDeMil.activeSelf)
        {
            targetNumber = Random.Range(10000, 100000);
        }
        else if (!unidadDeMil.activeSelf && !decenaDeMil.activeSelf)
        {
            targetNumber = Random.Range(0, 1000);
        }
        
        targetNumberText.text = " " + targetNumber;
    }

    void PlayVideo()
    {
        videoImage.gameObject.SetActive(true);  // Mostrar el RawImage
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoImage.gameObject.SetActive(false);  // Ocultar el RawImage cuando termina el video
        SceneManager.LoadScene("ruleta");
    }
}