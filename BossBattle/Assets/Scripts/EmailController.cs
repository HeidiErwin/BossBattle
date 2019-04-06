using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailController: MonoBehaviour
{
    public GameObject currentEmail;
    private bool displayingEmail = false;
    public List<GameObject> inbox;

    public Sprite closed;
    public Sprite newMessage;
    public Sprite open;
    public Animation flashing;

    void Update()
    {
        if (!displayingEmail && inbox.Count > 0) {
            // play flashing animation
        } else {

        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (!displayingEmail && inbox.Count > 0) {
                currentEmail = inbox[0];
                currentEmail.SetActive(true);
                displayingEmail = true;
                GetComponent<Image>().sprite = open;
                inbox.RemoveAt(0);
            } else if (!displayingEmail && inbox.Count <= 0) {
                // TODO: play "inbox empty!" error sound
            }
            else {
                currentEmail.SetActive(false);
                displayingEmail = false;
                GetComponent<Image>().sprite = closed;
            }
        }
    }

    void AddEmailToInbox (GameObject email) {
        inbox.Add(email);
    }
}