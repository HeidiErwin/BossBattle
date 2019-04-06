﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {
    [SerializeField] private float confidence = 1.0f; // confidence ranges from 0.0.to 1.0
    [SerializeField] private int tasksHandling; // tracks number of things the boss is currently working on
    [SerializeField] private int numTasksCapableOfHandling; // max number of tasks boss can handle before being overwhelmed
    [SerializeField] private ConfidenceBar confidenceBar;
    [SerializeField] private GameController gameController;
    [SerializeField] GameObject bossHeadUI;

    [SerializeField] private Sprite happyBoss;
    [SerializeField] private Sprite neutralBoss;
    [SerializeField] private Sprite distressedBoss;
    [SerializeField] private Sprite hangryBoss;
    [SerializeField] private Sprite happyBossHead;
    [SerializeField] private Sprite neutralBossHead;
    [SerializeField] private Sprite distressedBossHead;
    [SerializeField] private Sprite hangryBossHead;

    private float timeLeft = 0.0f;
    private float bufferTime = 8.0f;
    public static bool busyMutex = false;
    private Slider workBar;
    private List<float> workQueueTimes = new List<float>();

    public void Start() {
        this.workBar = transform.Find("WorkBar").Find("Slider").gameObject.GetComponent<Slider>();
    }

    void Update() {
        if (timeLeft <= 0 && isBusy()) {
            workQueueTimes.Remove(workQueueTimes[0]);

            if (isBusy()) {
                this.bufferTime = 8.0f;
                startWorkingOnTask();
            }
        } else if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            workBar.value = timeLeft;
        }

        if (!isBusy()) {
            bufferTime -= Time.deltaTime;
            if (bufferTime < 0) {
                SetConfidence(confidence - .001f);
            }
        }

        if (tasksHandling > numTasksCapableOfHandling) {
            SetConfidence(confidence - .01f);
            confidenceBar.SetConfidence(confidence);
        }

        if (confidence > .75f) {
            this.GetComponent<SpriteRenderer>().sprite = happyBoss;
            bossHeadUI.GetComponent<Image>().sprite = happyBossHead;
        } else if (confidence > .5f) {
            this.GetComponent<SpriteRenderer>().sprite = neutralBoss;
            bossHeadUI.GetComponent<Image>().sprite = neutralBossHead;
        } else if (confidence > .25f) {
            this.GetComponent<SpriteRenderer>().sprite = distressedBoss;
            bossHeadUI.GetComponent<Image>().sprite = distressedBossHead;
        } else {
            this.GetComponent<SpriteRenderer>().sprite = hangryBoss;
            bossHeadUI.GetComponent<Image>().sprite = hangryBossHead;
        }
    }

    public void SetConfidence(float conf) {
        if (confidence >= 0) {
            confidence = conf;
            confidenceBar.SetConfidence(conf);

        }
        if (confidence >= 1.0f) {
            confidence = 1.0f;
            confidenceBar.SetConfidence(1.0f);
        }
        if (confidence <= 0)
        {
            gameController.LoseState(0);
        }
    }

    public float GetConfidence() {
        return confidence;
    }

    public bool isBusy() {
        return this.workQueueTimes.Count > 0;
    }

    public bool queueFull() {
        return this.workQueueTimes.Count >= numTasksCapableOfHandling;
    }

    public void startWorkingOnTask() {
        Debug.Log("Started working on task with time = " + workQueueTimes[0]);
        timeLeft = workQueueTimes[0];

        workBar.maxValue = timeLeft;
        workBar.value = timeLeft;
    }

    public bool assignTask(float length) {
        Debug.Log("assigning task with length = " + length);
        workQueueTimes.Add(length);

        if (workQueueTimes.Count == 1) {
            startWorkingOnTask();
        }
        return true;
    }
}
