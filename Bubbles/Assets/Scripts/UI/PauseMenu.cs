using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [SerializeField] private GameObject pauseMenuUI = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        gameIsPaused = false;
        Time.timeScale = 1f;
    }
    
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        // Time.timeScale = 1f;
        // SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Menu");
        Application.Quit();
    }
}
