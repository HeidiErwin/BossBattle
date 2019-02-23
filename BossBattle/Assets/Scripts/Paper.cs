using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour {

	private GameObject player;
	private GameObject menu;

	private GameController manager;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find("Player");
		this.menu = transform.Find("Menu").gameObject;
		this.manager = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(player.transform.position,transform.position) < 1) {
			menu.SetActive(true);
			handleOptions();
		} else {
			menu.SetActive(false);
		}
	}

	void handleOptions() {
		if (Input.GetKeyDown(KeyCode.W)) {
			workOnItYourself();
		} else if (Input.GetKeyDown(KeyCode.A)) {
			assignToNPC();
		} else if (Input.GetKeyDown(KeyCode.S)) {
			sendToBoss();
		} else if (Input.GetKeyDown(KeyCode.D)){
			denyRequest();
		}
	}

	void workOnItYourself() {
		Debug.Log("work");
		player.GetComponent<Player>().assignTask(10);
		Destroy(gameObject);
	}

	void assignToNPC() {
		CoWorker c = manager.getAvailableWorker();
		if (c != null) {
			c.assignTask(10);
			Destroy(gameObject);
		} else {
			Debug.Log("No Workers!");
		}
		
	}

	void sendToBoss() {
		Debug.Log("send");
		Destroy(gameObject);
	}

	void denyRequest() {
		Debug.Log("deny");
		Destroy(gameObject);
	}

}
