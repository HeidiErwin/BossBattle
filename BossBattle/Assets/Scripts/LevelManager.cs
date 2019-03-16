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
        SceneManager.LoadScene("Heidi");
    }

    public void ShowCredits() {
        SceneManager.LoadScene("Credits");
    }
}
