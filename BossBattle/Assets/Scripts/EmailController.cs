using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailController: MonoBehaviour
{
    public GameObject currentEmail;
    private bool displayingEmail = false;
    public List<GameObject> inbox;
    [SerializeField] private GameController controller;

    [SerializeField] private GameObject goodEmail1;
    [SerializeField] private GameObject goodEmail2;

    [SerializeField] private GameObject badEmail1;
    [SerializeField] private GameObject badEmail2;

    public Sprite closed;
    public Sprite newMessage;
    public Sprite open;
    private Animator animator;

    private bool pauseGame = false;
    private bool unpauseGame = false;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (!displayingEmail && inbox.Count > 0) { //show email
                currentEmail = inbox[0];
                currentEmail.SetActive(true);
                displayingEmail = true;
                animator.SetBool("readingMail", true);
                inbox.RemoveAt(0);
                GetComponent<Image>().sprite = open;
                animator.enabled = false;
                controller.Pause();

            } else if (!displayingEmail && inbox.Count <= 0) { // no email to show
                // TODO: play "inbox empty!" error sound
            }
            else { // hide email
                currentEmail.SetActive(false);
                displayingEmail = false;
                animator.SetBool("readingMail", false);
                animator.enabled = true;
                controller.Unpause();
            }
        }

        if (!displayingEmail && inbox.Count > 0) {
            animator.SetBool("inboxEmpty", false);
            animator.SetBool("readingMail", false);
        } else if (displayingEmail) {
            animator.SetBool("readingMail", true);
        } else {
            animator.SetBool("inboxEmpty", true);
        }
    }

    private void LateUpdate() {
        if (pauseGame) {
            controller.Pause();
            pauseGame = false;
        } else if (unpauseGame) {
            controller.Unpause();
            unpauseGame = false;
        }
    }

    public void AddEmailToInbox(int num, bool good) {
        if (num == 1) {
            if (good) {
                inbox.Add(goodEmail1);
                Debug.Log("good email 1 added");
            } else {
                inbox.Add(badEmail1);
                Debug.Log("bad email 1 added");
            }
        } else if (num == 2) {
            if (good) {
                inbox.Add(goodEmail2);
                Debug.Log("good email 2 added");
            } else {
                inbox.Add(badEmail2);
                Debug.Log("bad email 2 added");
            }
        } else {
            Debug.LogError("Invalid Email Number");
        }
    }
}