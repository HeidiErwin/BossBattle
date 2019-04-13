using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoWorker : MonoBehaviour
{
    private float timeLeft = 0.0f; // Time left until this worker finishes a task.
    private bool busy = false;
    private Slider workBar;
    public static bool busyMutex = false;

    private GameController manager;

    void Start() {
        this.manager = GameObject.Find("GameController").GetComponent<GameController>();
        this.workBar = transform.Find("WorkBar").Find("Slider").gameObject.GetComponent<Slider>();
    }
    void Update()
    {
        if (!manager.IsPaused()) {
            if (timeLeft < 0 && busy) {
                this.busy = false;
                manager.addWorker(this);
            } else if (timeLeft > 0) {
                timeLeft -= Time.deltaTime;
                workBar.value = timeLeft;
            }
        }
    }

    // Returns true if task was assigned successfully, and false if worker is busy.
    public bool assignTask(float length) {
        if (!busy) {
            busy = true;
            workBar.maxValue = length;
            workBar.value = length;
            this.timeLeft = length;
            return true;
        }
        return false;
    }
}
