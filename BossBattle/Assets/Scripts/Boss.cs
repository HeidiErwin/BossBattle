using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private float confidence = 1.0f; // confidence ranges from 0.0.to 1.0
    [SerializeField] private int tasksHandling; // tracks number of things the boss is currently working on
    [SerializeField] private int numTasksCapableOfHandling; // max number of tasks boss can handle before being overwhelmed
    [SerializeField] private ConfidenceBar confidenceBar;
    [SerializeField] private GameController gameController;


    void Update()
    {
        if (tasksHandling > numTasksCapableOfHandling) {
            SetConfidence(confidence - .001f);
            confidenceBar.SetConfidence(confidence);
        }
    }

    public void SetConfidence(float conf) {
        if (confidence >= 0) {
            confidence = conf;
            confidenceBar.SetConfidence(conf);
            if (confidence <= 0) {
                gameController.LoseState(0);
            }
        }
    }

    public float GetConfidence() {
        return confidence;
    }
}
