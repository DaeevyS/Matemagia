using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class OrganizarGameController : MonoBehaviour
{
    [SerializeField] private NumberGenerator numberGenerator;
    [SerializeField] private DragAndDropManager dragAndDropManager;
    [SerializeField] private Text numberDisplay;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button newNumberButton;
    [SerializeField] private Text resultText;

    [Header("Video Configuration")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoImage;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance; // Obtener la instancia de GameManager

        confirmButton.onClick.AddListener(ConfirmAnswer);
        newNumberButton.onClick.AddListener(GenerateNewNumber);
        videoPlayer.loopPointReached += OnVideoEnd;
        videoImage.gameObject.SetActive(false);
        GenerateNewNumber();
    }

    private void ConfirmAnswer()
    {
        int formedNumber = dragAndDropManager.GetFormedNumber();
        int generatedNumber = numberGenerator.GetGeneratedNumber();

        if (formedNumber == generatedNumber)
        {
            resultText.text = "¡Correcto!";
            PlayVideo();
            // Notificar al GameManager para actualizar el puntaje (e.g., 1 punto por respuesta correcta)
            if (gameManager != null)
            {
                gameManager.UpdateScore(1);
            }
        }
        else
        {
            resultText.text = "Incorrecto. Intenta de nuevo.";
        }
    }

    private void GenerateNewNumber()
    {
        numberGenerator.GenerateNumber();
        numberDisplay.text = numberGenerator.GetNumberInWords();
        resultText.text = "";

        // Aquí deberías resetear las posiciones de los dígitos
        ResetDigitPositions();
    }

    // Método para restablecer las posiciones de los dígitos
    private void ResetDigitPositions()
    {
        foreach (Transform spot in dragAndDropManager.dropSpots)
        {
            // Si el spot tiene un hijo, devuelve el hijo a su posición original
            if (spot.childCount > 0)
            {
                GameObject digit = spot.GetChild(0).gameObject;
                DraggableDigit draggableDigit = digit.GetComponent<DraggableDigit>();
                if (draggableDigit != null)
                {
                    draggableDigit.ResetPosition(); // Restablece la posición del dígito
                }
            }
        }
    }

    private void PlayVideo()
    {
        videoImage.gameObject.SetActive(true);
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        videoImage.gameObject.SetActive(false);
        SceneManager.LoadScene("ruleta");
    }
}