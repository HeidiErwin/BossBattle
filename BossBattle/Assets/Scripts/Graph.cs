using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private GameObject timeMarker;
    [SerializeField] private DrawLine line; // the dynamic line that gets drawn at certain intervals
    private int numLineSegments = 5;
    private float distToMoveTimeMarkerPerSec;
    private float timer = 0.0f;
    private float graphWidth;
    private float graphHeight;
    private GameObject mostRecentPoint;
    private float markerMovementThroughCurrentSection = 0;
    
    // these will go in controller but someone else editing that script rn
    private int tasksFinished = 0; 
    private int totalPossibleTasks = 10;
    private float secondsPerLevel = 20f;

    // Start is called before the first frame update
    void Start()
    {
        timeMarker = GameObject.Find("TimeMarker");

        graphWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        graphHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        Debug.Log("height = " + graphHeight);
        distToMoveTimeMarkerPerSec = graphWidth/secondsPerLevel;
    }

    // Update is called once per frame
    void Update()
    {
        timeMarker.transform.position = 
            new Vector3(timeMarker.transform.position.x + distToMoveTimeMarkerPerSec * Time.deltaTime,
            timeMarker.transform.position.y, timeMarker.transform.position.z);

        markerMovementThroughCurrentSection += distToMoveTimeMarkerPerSec * Time.deltaTime;

        if (markerMovementThroughCurrentSection >= graphWidth/numLineSegments) {
            PlaceDotOnGraph(timeMarker.transform.position.x);

            Transform prevLineEnd = line.GetEnd();
            //GameObject segment = new GameObject();
            //segment.SetActive(false);
            GameObject segment = Instantiate(Resources.Load("LineRenderer")) as GameObject;
            //GameObject segment = new GameObject();
            line = segment.GetComponent<DrawLine>();

            Debug.Log(prevLineEnd);
            line.SetStart(prevLineEnd);
            line.SetDest(mostRecentPoint.transform);
            segment.SetActive(true);
            line.SetDrawing(true);
            markerMovementThroughCurrentSection = 0;
        }

        // change later, this is for testing that graph responds to finished tasks
        if (Input.GetKeyDown(KeyCode.T)) {
            tasksFinished++;
            PlaceDotOnGraph(timeMarker.transform.position.x);
        }
    }

    private void PlaceDotOnGraph(float timeMarkerX) {
        GameObject point = Instantiate(Resources.Load("Point")) as GameObject;
        float pointX = timeMarkerX;
        float graphBottomLeft = this.transform.position.y - graphHeight / 2;
        float pointY = graphBottomLeft + (graphHeight / totalPossibleTasks * tasksFinished);
        point.transform.position = new Vector3(pointX, pointY, 0);
        mostRecentPoint = point;
    }

    public float GetWidth() {
        return GetComponent<Sprite>().bounds.size.x;
    }
}
