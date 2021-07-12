using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInalUI : MonoBehaviour
{
    public void PlayGame()
    {
        GameManager.currentLevel = 1;
        GameManager.GoToScene(GameManager.currentLevel);
    }

    public void LoadMenu()
    {
        GameManager.GoToScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
