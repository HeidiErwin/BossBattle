using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour {

    public float speed = 10f;
    public int isMoving = 1;


    public float workTime;
	private GameObject player;
	private GameObject menu;
    private bool hasGridMutex;

	private GameController manager;
    private GridManager gridManager;
    private Rigidbody2D body;
    private Boss boss;
    private Node prevTarget;

    private List<Node> path;
    private int pathIndex;
    private Graph graph;

    public List<Sprite> sprites;

	public static bool busyMutex;

	// Use this for initialization
	void Start () {
		this.player = GameObject.Find("Player");
		this.menu = transform.Find("Menu").gameObject;
		this.manager = GameObject.Find("GameController").GetComponent<GameController>();
        this.gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
        this.body = GetComponent<Rigidbody2D>();
        this.boss = GameObject.Find("Boss").GetComponent<Boss>();
        this.path = gridManager.FindPath(transform.position, boss.transform.position);
    }

    // Update is called once per frame
    void Update () {
        MoveTowardsTarget();
        if (Vector3.Distance(player.transform.position, transform.position) < 1) {
            menu.SetActive(true);
            handleOptions();
        } else {
            menu.SetActive(false);
        }
	}

    private void MoveTowardsTarget() {
        if (pathIndex < path.Count) {
            gridManager.DrawPath(path);

            Node target = path[pathIndex];
            Vector2 difference = target.getPosition() - (Vector2)transform.position;
            Vector2 direction = Vector3.Normalize(difference);
            body.velocity = direction * speed;

            if (difference.magnitude < 0.05f) {
                if (pathIndex + 1 < path.Count) {
                    Node nextTarget = path[pathIndex + 1];

                    if (!gridManager.gridOccupied[nextTarget.xIndex, nextTarget.yIndex])
                    {
                        gridManager.gridOccupied[nextTarget.xIndex, nextTarget.yIndex] = true;
                        gridManager.gridOccupied[target.xIndex, target.yIndex] = false;
                        pathIndex += 1;
                    }
                    else  // Does not have lock
                    {
                        this.path = gridManager.FindPath(transform.position, boss.transform.position);
                        body.velocity = Vector2.zero;
                        pathIndex = 0;
                    }
                } else {
                    UnlockMutex();
                    body.velocity = Vector2.zero;
                    sendToBoss();
                    return;
                }
            } 
        }
    }

    void UnlockMutex() {
        Node target = path[pathIndex];
        gridManager.gridOccupied[target.xIndex, target.yIndex] = false;
    }

    void handleOptions() {
		if (Input.GetKeyDown(KeyCode.W)) {
			workOnItYourself();
		} else if (Input.GetKeyDown(KeyCode.A)) {
			assignToNPC();
		} else if (Input.GetKeyDown(KeyCode.S)) {
            if (!boss.queueFull()) {
                sendToBoss();
            }
		} else if (Input.GetKeyDown(KeyCode.D)){
			denyRequest();
		}
	}

	void workOnItYourself() {
		Debug.Log("work");
		if (player.GetComponent<Player>().assignTask(workTime)) {
            UnlockMutex();
            Destroy(gameObject);
		}
	}

	void assignToNPC() {
		if (!CoWorker.busyMutex) {
            CoWorker c = manager.getAvailableWorker();
			if (c != null) {
				if (c.assignTask(workTime)) {
					CoWorker.busyMutex = true;
					Invoke("setCoWorkerBusyMutex", 0.0001f);
                    UnlockMutex();
                    Destroy(gameObject, 0.001f);
				}
			} else {
				Debug.Log("No Workers!");
			}
		}
		
	}

	void sendToBoss() {
        Debug.Log("Sending to boss");
		if (!Boss.busyMutex && !this.boss.queueFull()) {
			Boss.busyMutex = true;
			this.boss.assignTask(workTime);
            boss.SetConfidence(boss.GetConfidence() + 0.1f);
            Invoke("setBossBusyMutex", 0.0001f);
            UnlockMutex();
            Destroy(gameObject, 0.001f);
        }
        if (boss.queueFull()) {
            boss.SetConfidence(boss.GetConfidence() - 0.001f);
        }
		
	}

	void denyRequest() {
		if (!Paper.busyMutex) {
			Paper.busyMutex = true;
			Invoke("setBusyMutex", 0.000f);
            UnlockMutex();
            Destroy(gameObject, 0.001f);
		}
	}

	private void setBusyMutex()
    {
		Paper.busyMutex = false;
    }

	private void setBossBusyMutex()
    {
		Boss.busyMutex = false;
    }

	private void setCoWorkerBusyMutex()
    {
		CoWorker.busyMutex = false;
    }

}
