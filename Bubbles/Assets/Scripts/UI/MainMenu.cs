using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.GoToScene(GameManager.currentLevel);

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
