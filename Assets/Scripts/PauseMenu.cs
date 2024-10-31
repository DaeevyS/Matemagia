using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;

    // Update is called once per frame
    void Update()
    {
        // Additional logic can be handled here if needed
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;  // Pauses the game
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;  // Resumes the game
    }

    // Function to restart the current scene
    public void RestartScene()
    {
        Time.timeScale = 1;  // Make sure the time scale is set to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reloads the current scene
    }

    // Function to go back to the main menu (assuming the main menu is in build index 0)
    public void GoToMainMenu()
    {
        Time.timeScale = 1;   //Reset time scale to normal in case game is paused
        SceneManager.LoadScene("Menu");  // Loads the scene called "Menu"
        Debug.Log("Menu"); // For testing purposes in the editor
    }
}
