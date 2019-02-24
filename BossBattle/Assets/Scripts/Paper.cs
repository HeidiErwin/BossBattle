using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour {

    public float speed = 0.5f;
	private GameObject player;
	private GameObject menu;

	private GameController manager;
    private GridManager gridManager;
    private Rigidbody2D body;
    private GameObject boss;

    private List<Node> path;
    private int pathIndex;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find("Player");
		this.menu = transform.Find("Menu").gameObject;
		this.manager = GameObject.Find("GameController").GetComponent<GameController>();
        this.gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        this.body = GetComponent<Rigidbody2D>();
        this.boss = GameObject.Find("Boss");
        this.path = gridManager.FindPath(transform.position, boss.transform.position);
    }
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(player.transform.position,transform.position) < 1) {
			menu.SetActive(true);
			handleOptions();
		} else {
			menu.SetActive(false);
		}
        MoveTowardsTarget();
	}

    private void MoveTowardsTarget() {
        if (pathIndex < path.Count) {
            gridManager.DrawPath(path);

            Node target = path[pathIndex];
            Vector2 difference = target.getPosition() - (Vector2)transform.position;
            Vector2 direction = Vector3.Normalize(difference);
            body.velocity = direction * speed;
            if (difference.magnitude < 0.5f) {
                pathIndex += 1;
            }
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
