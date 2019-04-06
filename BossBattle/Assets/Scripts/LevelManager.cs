using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void QuitGame() {
        Application.Quit();
    }

    public void BackToTitle() {
        SceneManager.LoadScene("Title");
    }

    public void StartGame() {
        SceneManager.LoadScene("PlaytestingScene");
    }

    public void ShowCredits() {
        SceneManager.LoadScene("Credits");
    }
}
