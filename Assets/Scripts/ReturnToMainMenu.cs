using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void ReturnToMain()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}