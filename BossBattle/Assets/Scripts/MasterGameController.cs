using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;
using System;

public class MasterGameController : MonoBehaviour
{
    public int quarter = 1;
    private int q1Quota;
    private int q2Quota;
    private int q3Quota;
    private int q4Quota;

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
            SceneManager.LoadScene("FinalRundown");
        } else {
            SceneManager.LoadScene("Level" + quarter, LoadSceneMode.Additive);
        }
    }

    public void SetQuota (int quarter, int quota) {
        switch (quarter) {
            case 1:
                q1Quota = quota;
                break;
            case 2:
                q2Quota = quota;
                break;
            case 3:
                q3Quota = quota;
                break;
            case 4:
                q4Quota = quota;
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }
    }
}
