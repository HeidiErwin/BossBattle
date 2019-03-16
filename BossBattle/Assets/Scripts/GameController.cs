using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Boss boss;

    private float timeBetweenSpawns = 0.3f;

    private float timer = 0.0f; // Used as a counter in update
    private List<GameObject> spawnLocations;
    private List<CoWorker> availableWorkers;
    [SerializeField] GameObject confidenceTooLow;
    [SerializeField] GameObject quarterRundown;
    [SerializeField] float secondsRemaining = 120.0f; // change to 120 eventually
    [SerializeField] Text timerText;
    [SerializeField] Text workCountText;
    private int workCount = 2;
    private UnityEngine.Object[] papers;

    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<Boss>();
        availableWorkers = new List<CoWorker>();
        this.spawnLocations = new List<GameObject>();
        processWorkers(); // Add all workers to the list
        foreach(Transform child in GameObject.Find("SpawnLocations").transform) {
            this.spawnLocations.Add(child.gameObject);
        }
        timerText.text = secondsRemaining.ToString();
        papers = Resources.LoadAll("Paper");
    }

    // Update is called once per frame
    void Update()
    {
        // placeholder to test confidence bar
        if (timer > 0) {
            timer -= Time.deltaTime;
        } else {
            timer = timeBetweenSpawns;
            //GameObject paper = Resources.Load("Paper/Paper" + UnityEngine.Random.Range(0, 3)) as GameObject;
            UnityEngine.Object paper = papers[UnityEngine.Random.Range(0, papers.Length)];
            Instantiate(paper, this.spawnLocations[UnityEngine.Random.Range(0, this.spawnLocations.Count)].transform.position, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.T)) {  //placeholder to test work count
            workCount++;
        }
        if (Input.GetKey(KeyCode.Space)) {
            boss.SetConfidence(boss.GetConfidence() - .01f);
        }
        if (secondsRemaining > 0) {
            secondsRemaining -= Time.deltaTime;
            timerText.text = Mathf.Round(secondsRemaining).ToString();
        } else {
            DisplayQuarterRundown();
        }
    }

    private void DisplayQuarterRundown() {
        workCountText.text = workCount.ToString();
        quarterRundown.SetActive(true);
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
        foreach(Transform child in GameObject.Find("Coworkers").transform) {
            this.availableWorkers.Add(child.GetComponent<CoWorker>());
        }
    }


}
