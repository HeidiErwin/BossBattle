using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MasterGameController : MonoBehaviour
{
    public int quarter = 1;
    public int q1Quota;
    public int q2Quota;
    public int q3Quota;
    public int q4Quota;
    public int q1Score;
    public int q2Score;
    public int q3Score;
    public int q4Score;

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
        //if (quarter == 5) {
        //    SceneManager.LoadScene("FinalRundown");
            //GameObject.Find("q1Score").GetComponent<Text>().text = q1Score.ToString() + " / " + q1Quota.ToString();
            //GameObject.Find("q2Score").GetComponent<Text>().text = q2Score.ToString() + " / " + q2Quota.ToString();
            //GameObject.Find("q3Score").GetComponent<Text>().text = q3Score.ToString() + " / " + q3Quota.ToString();
            //GameObject.Find("q4Score").GetComponent<Text>().text = q4Score.ToString() + " / " + q4Quota.ToString();
            //GameObject.Find("finalScore").GetComponent<Text>().text = CalculateFinalScore();
            //Debug.Log(q1Score.ToString() + " / " + q1Quota.ToString());
            //Debug.Log(q2Score.ToString() + " / " + q2Quota.ToString());
            //Debug.Log(q3Score.ToString() + " / " + q3Quota.ToString());
            //Debug.Log(q4Score.ToString() + " / " + q4Quota.ToString());
            //Debug.Log(CalculateFinalScore());
        //} else {
            SceneManager.LoadScene("Level" + quarter, LoadSceneMode.Additive);
        //}
    }

    public string CalculateFinalScore() {
        return (q1Score + q2Score + q3Score + q4Score).ToString() + " / " + (q1Quota + q2Quota + q3Quota + q4Quota).ToString();
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

    public void SetScore (int quarter, int score) {
        switch (quarter) {
            case 1:
                q1Score = score;
                break;
            case 2:
                q2Score = score;
                break;
            case 3:
                q3Score = score;
                break;
            case 4:
                q4Score = score;
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }
    }
}
