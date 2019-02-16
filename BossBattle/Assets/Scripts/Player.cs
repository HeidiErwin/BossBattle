using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 velocity;
    private int direction; 
    protected float speed = 2.0f;

    //constants
    public const int NORTH = 0;
    public const int EAST = 1;
    public const int SOUTH = 2;
    public const int WEST = 3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        transform.Translate(velocity * speed * Time.deltaTime);
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
}
