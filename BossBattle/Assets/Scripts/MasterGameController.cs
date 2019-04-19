using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class MasterGameController : MonoBehaviour
{
    public int quarter = 1;
    public bool q1QuotaMet = false;
    public bool q2QuotaMet = false;
    public bool q3QuotaMet = false;
    public bool q4QuotaMet = false;

    void Start()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    }

    public void NextLevel() {
        SceneManager.UnloadSceneAsync("Level" + quarter);
        quarter += 1;
        if (quarter == 5) {
            SceneManager.LoadScene("Title");
        } else {
            SceneManager.LoadScene("Level" + quarter, LoadSceneMode.Additive);
        }
    }
}
