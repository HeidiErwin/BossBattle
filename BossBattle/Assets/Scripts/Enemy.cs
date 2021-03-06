﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   //private float interactDist = 3;
    private float timer = 0.0f;
    private GameObject gridTarget;

    // Start is called before the first frame update
    void Start()
    {
        gridTarget = GetClosestGrid();
    }

    private GameObject GetClosestGrid() {
        GameObject[] gridElements = GameObject.FindGameObjectsWithTag("grid");
        GameObject best = gridElements[0];
        float bestDistance = 0;
        for (int i = 0; i < gridElements.Length; i++)
        {
            float distance = (gridElements[i].transform.position - transform.position).magnitude;
            if (distance < bestDistance) {
                bestDistance = distance;
                best = gridElements[i];
            }
        }
        return best;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if ((player.transform.position - this.transform.position).magnitude < 3)
        {
            if (Input.GetKeyDown(KeyCode.W)) 
            {
                // reject request
                Destroy(this.gameObject);

            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                // do it yourself
                timer += Time.deltaTime;
                if (timer > 2.0f)
                {
                    Destroy(this.gameObject);
                    // Remove the recorded 2 seconds.
                    timer = timer - 2.0f;
                }
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                // give to NPC
                timer += Time.deltaTime;
                if (timer > 3.0f)
                {
                    Destroy(this.gameObject);
                    // Remove the recorded 2 seconds.
                    timer = timer - 1.0f;
                }
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // send to boss
                Destroy(this.gameObject);
            }
        }
    }
}
