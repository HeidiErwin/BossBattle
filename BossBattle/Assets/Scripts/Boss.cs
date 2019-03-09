using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {
    [SerializeField] private float confidence = 1.0f; // confidence ranges from 0.0.to 1.0
    [SerializeField] private int tasksHandling; // tracks number of things the boss is currently working on
    [SerializeField] private int numTasksCapableOfHandling; // max number of tasks boss can handle before being overwhelmed
    [SerializeField] private ConfidenceBar confidenceBar;
    [SerializeField] private GameController gameController;

    private bool busy;
    private float timeLeft = 0.0f;
    private float bufferTime = 8.0f;
    public static bool busyMutex = false;
    private Slider workBar;

    public void Start() {
        this.workBar = transform.Find("WorkBar").Find("Slider").gameObject.GetComponent<Slider>();
    }

    void Update() {

        if (timeLeft < 0 && busy) {
            tasksHandling--;
            this.busy = false;
        } else if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            workBar.value = timeLeft;
        }

        if (!busy) {
            bufferTime -= Time.deltaTime;
            if (bufferTime < 0) {
                SetConfidence(confidence - .001f);
            }
        }

        if (tasksHandling > numTasksCapableOfHandling) {
            SetConfidence(confidence - .01f);
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

    public bool isBusy() {
        return this.busy;
    }

    public bool assignTask(float length) {
        if (!busy) {
            busy = true;
            workBar.maxValue = length;
            workBar.value = length;
            this.timeLeft = length;
            this.bufferTime = 8.0f;
            tasksHandling++;
            return true;
        }
        return false;
    }
}
