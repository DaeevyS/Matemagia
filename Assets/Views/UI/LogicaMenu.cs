using UnityEngine;
using UnityEngine.SceneManagement; // For changing scenes
using UnityEngine.UI;              // For working with UI components

public class LogicaMenu : MonoBehaviour
{
    public Canvas CanvasIniciar;    // The main menu canvas
    public Canvas CanvasMenu;    // The gameplay canvas (or the next canvas to show)

    // Function called when the "Start Game" button is pressed
    public void StartGame()
    {
        // Activate gameplay canvas and deactivate the main menu canvas
        CanvasMenu.gameObject.SetActive(true);
        CanvasIniciar.gameObject.SetActive(false);
    }

    // Function called when the "Exit" button is pressed
    public void ExitGame()
    {
        // Close the application
        Debug.Log("Exit Game pressed!"); // For testing purposes in the editor
        Application.Quit();              // Quits the application in build mode
    }

    // Function called when the "Next Scene" button is pressed
    public void LoadNextScene()
    {
        // Load the next scene in the build settings
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
