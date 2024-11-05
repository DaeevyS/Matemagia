using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton Instance

    private void Awake()
    {
        // Verifica si existe una instancia previa
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene el GameManager entre escenas
        }
        else
        {
            Destroy(gameObject); // Elimina cualquier duplicado del GameManager
        }

        // Suscribir al evento para detectar cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            ResetScoresAndTurn(); // Reinicia puntajes y turno al cargar el menú principal
        }
    }

    // Variables para almacenar los puntajes de los personajes
    private int squirrelScore = 0;
    private int dogScore = 0;
    private int birdScore = 0;
    private int fishScore = 0;

    private int currentTurn = 0; // Turno actual: 0 = Ardilla, 1 = Perro, 2 = Pájaro, 3 = Pez

    // Constante para la puntuación de victoria
    private const int WINNING_SCORE = 20;

    // Función para verificar la respuesta y actualizar el puntaje
    public void UpdateScore(bool correctAnswer)
    {
        if (correctAnswer)
        {
            switch (currentTurn)
            {
                case 0: // Ardilla
                    squirrelScore++;
                    CheckForWinner(squirrelScore, "GanadoraArdilla");
                    break;
                case 1: // Perro
                    dogScore++;
                    CheckForWinner(dogScore, "GanadoraPerro");
                    break;
                case 2: // Pájaro
                    birdScore++;
                    CheckForWinner(birdScore, "GanadoraPajaro");
                    break;
                case 3: // Pez
                    fishScore++;
                    CheckForWinner(fishScore, "GanadoraPez");
                    break;
            }
        }

        // Cambiar al siguiente turno (siempre cíclico)
        currentTurn = (currentTurn + 1) % 4;
    }

    // Verificar si algún personaje ha alcanzado la puntuación de victoria
    private void CheckForWinner(int score, string winnerScene)
    {
        if (score >= WINNING_SCORE)
        {
            // Cargar la escena específica del ganador
            SceneManager.LoadScene(winnerScene);
        }
    }

    // Función para restablecer los puntajes y el turno
    private void ResetScoresAndTurn()
    {
        squirrelScore = 0;
        dogScore = 0;
        birdScore = 0;
        fishScore = 0;
        currentTurn = 0;
    }

    private void OnDestroy()
    {
        // Desuscribir el evento para evitar referencias nulas
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
