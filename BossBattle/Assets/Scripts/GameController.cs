using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private Boss boss;

    private const int PLAYING_GAME = 0;
    private const int PAUSE = 1; // email or otherwise
    private const int DISPLAYING_RESULTS = 2;

    [SerializeField] private float timeBetweenSpawns = 1f;

    private float timer = 0.0f; // Used as a counter in update
    private List<GameObject> spawnLocations;
    private List<CoWorker> availableWorkers;
    [SerializeField] GameObject confidenceTooLow;
    [SerializeField] GameObject quarterRundown;
    [SerializeField] float secondsRemaining;
    [SerializeField] Text timerText;
    [SerializeField] Text workCountText;
    private int workCount = 0;
    private int quarter = 1;
    private UnityEngine.Object[] papers;
    private int state = PLAYING_GAME;

    void Start() {
        boss = GameObject.Find("Boss").GetComponent<Boss>();
        availableWorkers = new List<CoWorker>();
        this.spawnLocations = new List<GameObject>();
        processWorkers(); // Add all workers to the list
        foreach (Transform child in GameObject.Find("SpawnLocations").transform) {
            this.spawnLocations.Add(child.gameObject);
        }
        timerText.text = secondsRemaining.ToString();
        papers = Resources.LoadAll("Paper");
    }

    void Update() {
        if (state == PLAYING_GAME) {
            if (timer > 0) {
                timer -= Time.deltaTime;
            } else {
                timer = timeBetweenSpawns;
                //GameObject paper = Resources.Load("Paper/Paper" + UnityEngine.Random.Range(0, 3)) as GameObject;
                UnityEngine.Object paper = papers[UnityEngine.Random.Range(0, papers.Length)];
                //UnityEngine.Object paper = papers[0];
                Instantiate(paper, this.spawnLocations[UnityEngine.Random.Range(0, this.spawnLocations.Count)].transform.position, Quaternion.identity);
            }
            if (secondsRemaining > 0) {
                secondsRemaining -= Time.deltaTime;
                timerText.text = Mathf.Round(secondsRemaining).ToString();
            } else {
                Debug.Log("time's up");
                DisplayQuarterRundown();
            }
        } else if (state == DISPLAYING_RESULTS) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                HideQuarterRundown();
                //TODO: load new level (reset ui, etc.)
            }
        }
    }

    private void DisplayQuarterRundown() {
        state = DISPLAYING_RESULTS;
        workCountText.text = workCount.ToString();
        quarterRundown.SetActive(true);
        if (quarter == 1) {

        }
    }

    private void HideQuarterRundown() {
        state = PLAYING_GAME;
        //workCountText.text = workCount.ToString();
        quarterRundown.SetActive(false);
    }

    /**
     * A method called when the game ends because the player has failed
     * If reason is 0, the boss's confidence dropped to zero.
     * If reasons is 1, you failed to meet your quota. 
     */
    public void LoseState(int reason) {
        if (reason == 0 && state == PLAYING_GAME) {
            DisplayConfidenceTooLow();
        }
    }

    private void DisplayConfidenceTooLow() {
        GameObject.Find("GameplayUI").SetActive(false);
        confidenceTooLow.SetActive(true);

    }

    public void addWorker(CoWorker c) {
        availableWorkers.Add(c);
    }

    // Gets an available worker and removes it from the list of workers.
    public CoWorker getAvailableWorker() {
        if (availableWorkers.Count == 0) {
            return null;
        }
        CoWorker ret = availableWorkers[0];
        availableWorkers.RemoveAt(0);
        Debug.Log(availableWorkers.Count);
        return ret;
    }

    // Finds the list of workers in game and adds them to this manager's list
    // of workers.
    public void processWorkers() {
        foreach (Transform child in GameObject.Find("Coworkers").transform) {
            this.availableWorkers.Add(child.GetComponent<CoWorker>());
        }
    }

    public void Pause() {
       if (state == PLAYING_GAME) {
            state = PAUSE;
        }
    }

    public void Unpause() {
        state = PLAYING_GAME;
    }

    public bool IsPaused() {
        return (state == PAUSE);
    }
}
