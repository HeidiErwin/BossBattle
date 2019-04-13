using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    private Vector2 velocity;
    private int direction; 
    protected float speed = 5.0f;
    private SpriteRenderer spriteRenderer;
    private GameController controller;

    //constants
    public const int NORTH = 0;
    public const int EAST = 1;
    public const int SOUTH = 2;
    public const int WEST = 3;

    private Slider workBar; // For when you decide to work by yourself.
    private float timeLeft = 0f;
    private bool busy = false;

    [SerializeField] private Sprite front;
    [SerializeField] private Sprite right;
    [SerializeField] private Sprite back;
    [SerializeField] private Sprite left;

    // Start is called before the first frame update
    void Start()
    {
        this.controller = GameObject.Find("GameController").GetComponent<GameController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        workBar = transform.Find("WorkBar").Find("Slider").gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.IsPaused()) {
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
    }

    void GetInput() {
        velocity = Vector2.zero;
            if (Input.GetKey(KeyCode.UpArrow)) {
                direction = NORTH;
                spriteRenderer.sprite = back;
                velocity += Vector2.up;
            }

            if (Input.GetKey(KeyCode.RightArrow)) {
                direction = EAST;
                spriteRenderer.sprite = right;
                velocity += Vector2.right;
            }

            if (Input.GetKey(KeyCode.DownArrow)) {
                direction = SOUTH;
                spriteRenderer.sprite = front;
            velocity += Vector2.down;
            }

            if (Input.GetKey(KeyCode.LeftArrow)) {
                direction = WEST;
                spriteRenderer.sprite = left;
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
