using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NumberGenerator numberGenerator;
    [SerializeField] private DragAndDropManager dragAndDropManager;
    [SerializeField] private Text numberDisplay;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button newNumberButton;
    [SerializeField] private Text resultText;

    private void Start()
    {
        confirmButton.onClick.AddListener(ConfirmAnswer);
        newNumberButton.onClick.AddListener(GenerateNewNumber);
        GenerateNewNumber();
    }

    private void ConfirmAnswer()
    {
        int formedNumber = dragAndDropManager.GetFormedNumber();
        int generatedNumber = numberGenerator.GetGeneratedNumber();

        if (formedNumber == generatedNumber)
        {
            resultText.text = "¡Correcto!";
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
}