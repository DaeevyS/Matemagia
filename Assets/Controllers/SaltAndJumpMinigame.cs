using UnityEngine;
using UnityEngine.UI;

public class SaltAndJumpMinigame : MonoBehaviour
{
    public Text questionText;
    public Button[] answerButtons;
    public Button resetButton;
    public Text resultText;
    public bool enableThousands = false;
    public bool enableTenThousands = false;

    private int[] numbers;
    private int correctAnswer;
    private bool isAnswerCorrect;

    private void Start()
    {
        GenerateNumbers();
        UpdateQuestionText();
        SetUpAnswerButtons();
        resetButton.onClick.AddListener(ResetGame);
    }

    private void GenerateNumbers()
    {
        // Generar números aleatorios para la suma
        numbers = new int[5];
        numbers[0] = Random.Range(1, 10);
        numbers[1] = Random.Range(0, 10);
        numbers[2] = Random.Range(0, 10);
        numbers[3] = enableThousands ? Random.Range(0, 10) : 0;
        numbers[4] = enableTenThousands ? Random.Range(0, 10) : 0;

        // Calcular la respuesta correcta
        correctAnswer = numbers[4] * 10000 + numbers[3] * 1000 + numbers[2] * 100 + numbers[1] * 10 + numbers[0];
    }

    private void UpdateQuestionText()
    {
        // Actualizar el texto de la pregunta
        questionText.text = "¿Cuál es el resultado de esta suma?\n";
        bool hasValue = false;
        for (int i = 4; i >= 0; i--)
        {
            if (i == 4 && enableTenThousands && numbers[i] > 0)
            {
                questionText.text += numbers[i] + "0000 + ";
                hasValue = true;
            }
            else if (i == 3 && enableThousands && numbers[i] > 0)
            {
                questionText.text += numbers[i] + "000 + ";
                hasValue = true;
            }
            else if (i == 2 && numbers[i] > 0)
            {
                questionText.text += numbers[i] + "00 + ";
                hasValue = true;
            }
            else if (i == 1 && numbers[i] > 0)
            {
                questionText.text += numbers[i] + "0 + ";
                hasValue = true;
            }
            else if (i == 0 && numbers[i] > 0)
            {
                questionText.text += numbers[i] + " = ";
                hasValue = true;
            }
        }

        // Si no hay valores en las posiciones superiores, omitir el "= "
        if (!hasValue)
        {
            questionText.text = questionText.text.Substring(0, questionText.text.Length - 3);
        }
    }

    private void SetUpAnswerButtons()
    {
        // Asignar respuestas a los botones
        int[] incorrectAnswers = GenerateIncorrectAnswers();

        answerButtons[0].GetComponentInChildren<Text>().text = correctAnswer.ToString();
        answerButtons[1].GetComponentInChildren<Text>().text = incorrectAnswers[0].ToString();
        answerButtons[2].GetComponentInChildren<Text>().text = incorrectAnswers[1].ToString();

        // Agregar listeners a los botones
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index));
        }
    }

    private int[] GenerateIncorrectAnswers()
    {
        // Generar respuestas incorrectas
        int[] incorrectAnswers = new int[2];
        int diff = Random.Range(1, 11);
        incorrectAnswers[0] = correctAnswer + diff;
        incorrectAnswers[1] = correctAnswer - diff;
        return incorrectAnswers;
    }

    private void CheckAnswer(int index)
    {
        // Verificar si la respuesta es correcta
        int selectedAnswer = int.Parse(answerButtons[index].GetComponentInChildren<Text>().text);
        if (selectedAnswer == correctAnswer)
        {
            // Respuesta correcta
            isAnswerCorrect = true;
            resultText.color = Color.green;
            resultText.text = "¡Correcto!";
        }
        else
        {
            // Respuesta incorrecta
            isAnswerCorrect = false;
            resultText.color = Color.red;
            resultText.text = $"Incorrecto. La respuesta correcta es {correctAnswer}";
        }

        // Mostrar el resultado
        resultText.gameObject.SetActive(true);
    }

    private void ResetGame()
    {
        // Generar una nueva ronda
        GenerateNumbers();
        UpdateQuestionText();
        SetUpAnswerButtons();
        resultText.gameObject.SetActive(false);
    }
}
