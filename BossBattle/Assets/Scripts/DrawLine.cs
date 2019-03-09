using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float counter;
    private float dist;

    private float lineDrawSpeed = 10f;
    private Transform start;
    private Transform destination;

    private GameObject graphBottomLeft;

    private bool isDrawing = false;

    void Start()
    {
        graphBottomLeft = GameObject.Find("GraphBottomLeft");
        lineRenderer = GetComponent<LineRenderer>();
        if (!start) {
            start = graphBottomLeft.transform;
        } 
        lineRenderer.SetPosition(0, start.position);
        lineRenderer.startWidth = .05f;
        lineRenderer.endWidth = .05f;
    }

    void FixedUpdate()
    {
        if (start && destination) {
            dist = Vector3.Distance(start.position, destination.position);
        }

        if ((isDrawing) && (counter < dist)) {
            counter += .1f / lineDrawSpeed;

            // lerp = linear interpolation, will give us value of counter but max bound dist and min bound 0
            float len = Mathf.Lerp(0, dist, counter);

            // animate the line being drawn
            Vector3 pointA = start.position;
            Vector3 pointB = destination.position;
            Debug.Log("here");

            // get unit vector in desired direction, multiple by desired length, add start point
            Vector3 pointAlongLine = len * Vector3.Normalize(pointB - pointA) + pointA;

            lineRenderer.SetPosition(1, pointAlongLine); 
        } else if (isDrawing) {
            isDrawing = false;
            start = destination;
        }
    }

    public void SetDest(Transform dest) {
        destination = dest;
    }

    public void SetStart(Transform s) {
        start = s;
    }

    public void SetDrawing(bool b) {
        isDrawing = b;
    }

    public Transform GetEnd() {
        return destination;
    }
}
