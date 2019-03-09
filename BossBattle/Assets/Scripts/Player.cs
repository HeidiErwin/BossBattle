using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    private Vector2 velocity;
    private int direction; 
    protected float speed = 5.0f;

    //constants
    public const int NORTH = 0;
    public const int EAST = 1;
    public const int SOUTH = 2;
    public const int WEST = 3;

    private Slider workBar; // For when you decide to work by yourself.
    private float timeLeft = 0f;
    private bool busy = false;


    // Start is called before the first frame update
    void Start()
    {
        workBar = transform.Find("WorkBar").Find("Slider").gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft < 0 && busy) {
            this.busy = false;
            workBar.gameObject.SetActive(false);
        } else if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            workBar.value = timeLeft;
        } else {
            GetInput();
            transform.Translate(velocity * speed * Time.deltaTime);
        }

        
    }

    void GetInput() {
        velocity = Vector2.zero;

            if (Input.GetKey(KeyCode.UpArrow)) {
                direction = NORTH;
                velocity += Vector2.up;
            }

            if (Input.GetKey(KeyCode.RightArrow)) {
                direction = EAST;
                velocity += Vector2.right;
            }

            if (Input.GetKey(KeyCode.DownArrow)) {
                direction = SOUTH;
                velocity += Vector2.down;
            }

            if (Input.GetKey(KeyCode.LeftArrow)) {
                direction = WEST;
                velocity += Vector2.left;
            }
    }

    public bool assignTask(float length) {
        if (!busy) {
            busy = true;
            timeLeft = length;
            workBar.gameObject.SetActive(true);
            workBar.maxValue = length;
            workBar.value = length;
            return true;
        }
        return false;
    }
}
