using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Boss boss;
    [SerializeField] GameObject confidenceTooLow;

    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    // Update is called once per frame
    void Update()
    {
        // placeholder to test confidence bar
        if (Input.GetKey(KeyCode.Space)) {
            boss.SetConfidence(boss.GetConfidence() - .01f);
            
        }
    }

    /**
     * A method called when the game ends because the player has failed
     * If reason is 0, the boss's confidence dropped to zero.
     * If reasons is 1, you failed to meet your quota. 
     */
    public void LoseState(int reason) {
        if (reason == 0) {
            DisplayConfidenceTooLow();
        }
    }

    private void DisplayConfidenceTooLow() {
        confidenceTooLow.SetActive(true);
    }
}
