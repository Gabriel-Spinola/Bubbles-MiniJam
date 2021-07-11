using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.GoToNextScene();

        if (PauseMenu.gameIsPaused) {
            PauseMenu.gameIsPaused = false;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
