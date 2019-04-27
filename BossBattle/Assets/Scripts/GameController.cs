using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
    private Boss boss;
    MasterGameController masterController;

    private const int PLAYING_GAME = 0;
    private const int PAUSE = 1; // email or otherwise
    private const int DISPLAYING_RESULTS = 2;

    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private EmailController emailController;

    private float timer = 0.0f; // Used as a counter in update
    [SerializeField] private float totalLevelLength; // num seconds in level
    private List<GameObject> spawnLocations;
    private List<CoWorker> availableWorkers;
    [SerializeField] GameObject confidenceTooLow;
    [SerializeField] GameObject quarterRundown;
    private float secondsRemaining;
    [SerializeField] int workQuota;
    [SerializeField] Text quotaText; // "/10" for instance, at top left of screen
    [SerializeField] Text timerText;
    [SerializeField] Text rundownQuotaText;
    private int workCount = 0;
    [SerializeField] public int quarter = 1;
    private UnityEngine.Object[] papers;
    private int state = PLAYING_GAME;

    private bool firstEmailSent = false;
    private bool secondEmailSent = false;
    private bool warningSoundPlayed = false;

    [SerializeField] Text workText; // part of gameplay 
    [SerializeField] Text workCountText; // final, for quarter rundown
    [SerializeField] GameObject rundownQ1Box;
    [SerializeField] GameObject rundownQ2Box;
    [SerializeField] GameObject rundownQ3Box;
    [SerializeField] GameObject rundownQ4Box;
    [SerializeField] GameObject successMessage;
    [SerializeField] GameObject failMessage;

    // Audio
    private AudioSource source;
    [SerializeField] private AudioClip lose;
    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip taskFinished;
    [SerializeField] private AudioClip scribbling;
    [SerializeField] private AudioClip newEmail;
    [SerializeField] private AudioClip openEmail;
    [SerializeField] private AudioClip closeEmail;
    [SerializeField] private AudioClip warning;

    void Start() {
        source = GetComponent<AudioSource>();
        SceneManager.SetActiveScene(gameObject.scene);
        secondsRemaining = totalLevelLength;
        boss = GameObject.Find("Boss").GetComponent<Boss>();
        availableWorkers = new List<CoWorker>();
        this.spawnLocations = new List<GameObject>();
        processWorkers(); // Add all workers to the list
        foreach (Transform child in GameObject.Find("SpawnLocations").transform) {
            this.spawnLocations.Add(child.gameObject);
        }
        timerText.text = secondsRemaining.ToString();
        quotaText.text = "/" + workQuota.ToString();
        papers = Resources.LoadAll("Paper");
        quarter = FindObjectOfType<MasterGameController>().quarter;
        masterController = GameObject.Find("MasterGameController").GetComponent<MasterGameController>();
    }

    void Update() {
        if (state == PLAYING_GAME) {
            workText.text = workCount.ToString();
            if (timer > 0) {
                timer -= Time.deltaTime;
            } else {
                timer = timeBetweenSpawns;
                //GameObject paper = Resources.Load("Paper/Paper" + UnityEngine.Random.Range(0, 3)) as GameObject;
                UnityEngine.Object paper = papers[UnityEngine.Random.Range(0, papers.Length)];
                //UnityEngine.Object paper = papers[0];
                Instantiate(paper, this.spawnLocations[UnityEngine.Random.Range(0, this.spawnLocations.Count)].transform.position, Quaternion.identity);
                Debug.Log("spawning!");
            }

            if (secondsRemaining > 0) {
                secondsRemaining -= Time.deltaTime;
                timerText.text = Mathf.Round(secondsRemaining).ToString();
                if (!warningSoundPlayed && secondsRemaining < 10.0f) {
                    source.PlayOneShot(warning, 1.0f);
                    warningSoundPlayed = true;
                    timerText.color = Color.red;
                }
            } else {
                Debug.Log("time's up");
                DisplayQuarterRundown();
            }

            SendEmailsAsAppropriate();
        } else if (state == DISPLAYING_RESULTS) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                HideQuarterRundown();
                masterController.NextLevel();
            }
        }
    }

    private void DisplayQuarterRundown() {
        state = DISPLAYING_RESULTS;
        workCountText.text = workCount.ToString();
        rundownQuotaText.text = "/" + workQuota.ToString();
        quarterRundown.SetActive(true);
        if (QuotaMet()) {
            successMessage.SetActive(true);
            failMessage.SetActive(false);
            source.PlayOneShot(win, 1.0f);
        } else {
            failMessage.SetActive(true);
            successMessage.SetActive(false);
            source.PlayOneShot(lose, 1.0f);
        }
        DisplayGraph();
    }

    private void DisplayGraph() {
        switch (masterController.quarter) {
            case 1:
                if (QuotaMet()) {
                    rundownQ1Box.GetComponent<Animator>().SetBool("quotaMet", true);
                    masterController.q1QuotaMet = true;
                }
                break;
            case 2:
                if (QuotaMet()) {
                    rundownQ2Box.GetComponent<Animator>().SetBool("quotaMet", true);
                    masterController.q2QuotaMet = true;
                }
                break;
            case 3:
                if (QuotaMet()) {
                    rundownQ3Box.GetComponent<Animator>().SetBool("quotaMet", true);
                    masterController.q3QuotaMet = true;
                }
                break;
            case 4:
                if (QuotaMet()) {
                    rundownQ4Box.GetComponent<Animator>().SetBool("quotaMet", true);
                    masterController.q4QuotaMet = true;
                }
                break;
            default:
                Console.WriteLine("Default case");
                break;
        }
    }
        private bool QuotaMet() {
        return (workCount >= workQuota);
    }

    private void HideQuarterRundown() {
        state = PLAYING_GAME;
        //workCountText.text = workCount.ToString();
        quarterRundown.SetActive(false);
    }

    public void IncreaseWorkCount() {
        workCount++;
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
        Time.timeScale = 0.0f;
    }

    public void Unpause() {
        state = PLAYING_GAME;
        Time.timeScale = 1.0f;
    }

    public bool IsPaused() {
        return (state == PAUSE);
    }

    public bool OnTrack() {
        float secondsIntoGame = totalLevelLength - secondsRemaining;
        return ((workCount / (float)workQuota) >= (secondsIntoGame / totalLevelLength));
    }

    private void SendEmailsAsAppropriate() {
        float secondsIntoGame = totalLevelLength - secondsRemaining;
        Debug.Log(secondsIntoGame/totalLevelLength);
        if ((secondsIntoGame / totalLevelLength) >= (1.0f/3.0f) && !firstEmailSent) {
            if (OnTrack()) {
                emailController.AddEmailToInbox(1, true);
            } else {
                emailController.AddEmailToInbox(1, false);
            }
            firstEmailSent = true;
            source.PlayOneShot(newEmail, 1.0f);
        } else if ((secondsIntoGame / totalLevelLength) >= (2.0f/3.0f) && !secondEmailSent) {
            if (OnTrack()) {
                emailController.AddEmailToInbox(2, true);
            } else {
                emailController.AddEmailToInbox(2, false);
            }
            secondEmailSent = true;
            source.PlayOneShot(newEmail, 1.0f);
        }
    }

    public void PlayTaskFinishedSound() {
        source.PlayOneShot(taskFinished, 1.0f);
    }

    public void PlayBossWorkingSound() {
        source.PlayOneShot(scribbling, .5f);
    }

    public void PlayEmailOpenSound() {
        source.PlayOneShot(openEmail, 1.0f);
    }

    public void PlayEmailCloseSound() {
        source.PlayOneShot(closeEmail, 1.0f);
    }
}
