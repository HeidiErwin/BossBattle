using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FinalRundownController : MonoBehaviour
{
    [SerializeField] private GameObject q1Score;
    [SerializeField] private GameObject q2Score;
    [SerializeField] private GameObject q3Score;
    [SerializeField] private GameObject q4Score;
    [SerializeField] private GameObject finalScore;
    private MasterGameController masterController;

    public void Start() {
    }

    public void Update() {
        masterController = GameObject.Find("MasterGameController").GetComponent<MasterGameController>();
        q1Score.GetComponent<Text>().text = masterController.q1Score.ToString() + " / " + masterController.q1Quota.ToString();
        q2Score.GetComponent<Text>().text = masterController.q2Score.ToString() + " / " + masterController.q2Quota.ToString();
        q3Score.GetComponent<Text>().text = masterController.q3Score.ToString() + " / " + masterController.q3Quota.ToString();
        q4Score.GetComponent<Text>().text = masterController.q4Score.ToString() + " / " + masterController.q4Quota.ToString();
        finalScore.GetComponent<Text>().text = masterController.CalculateFinalScore();
    }
  
}
