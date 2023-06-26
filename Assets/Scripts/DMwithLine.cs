using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMwithLine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject goalFX;

    [Header("Attributes")]
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float power = 2f;
    [SerializeField] private float maxGoalSpeed = 4f; // Max speed the disc can enter the basket and stay in
    [SerializeField] private float rotationMultiplier = .01f; // How fast the disc rotates. Higher = Faster
    [SerializeField] private int rotationDirection = -1; // Control the direction the disc spins. 1 for counter-clockwise, -1 for clockwise
    public Vector3 com; // Center of mass for the disc

    private bool isDragging;
    private bool inHole;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set center of mass on disc to make it rotate smoothly
        rb.centerOfMass = com;
    }

    // Update is called once per frame
    void Update()
    {
        // Call PlayerInput if the disc is not moving 
        if (rb.velocity.magnitude <= 0.2f) PlayerInput();
        
        // Check if player loses
        if (LevelManager.main.outOfStrokes && rb.velocity.magnitude <= 0.2f && !LevelManager.main.levelCompleted) {
            LevelManager.main.GameOver();
        }
    }

    // Get current player input
    private void PlayerInput (){
        Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPos);

        if (Input.GetMouseButtonDown(0) && distance < 0.5f) DragStart(); // Mouse first clicked
        if (Input.GetMouseButton(0) && isDragging) DragChange(inputPos); // Every frame mouse is held
        if (Input.GetMouseButtonUp(0) && isDragging) DragRelease(inputPos); // Mouse released
    }

    private void DragStart() {
        isDragging = true;
        lr.positionCount = 2;
    }

    private void DragChange(Vector2 pos) {
        Vector2 dir = (Vector2)transform.position - pos;

        // Aiming Line 
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude((dir * power) / 2, maxPower / 2)); // divide by 2 to shorten the aiming line
    }

    private void DragRelease(Vector2 pos) {
        float distance = Vector2.Distance((Vector2)transform.position, pos);
        isDragging = false;
        lr.positionCount = 0;

        // Cancel drag if it hasn't been far enough
        if (distance < 1f) {
            return;
        }

        // Increment stroke count
        LevelManager.main.IncreaseStroke();

        Vector2 dir = (Vector2)transform.position - pos;
        rb.velocity = Vector2.ClampMagnitude(dir * power, maxPower); // Clamp velocity to be below maxPower
        // Rotate the disc
        RotateDisc();
    }

    // Spin the disc in flight
    private void RotateDisc() {
        rb.AddTorque(rotationDirection * rotationMultiplier * rb.velocity.magnitude);
    }

    // Check if the player has completed the level
    private void CheckWinState() {
        if (inHole) return;

        if(rb.velocity.magnitude <= maxGoalSpeed) {
            inHole = true;

            rb.velocity = Vector2.zero;
            gameObject.SetActive(false);

            GameObject fx = Instantiate(goalFX, transform.position, Quaternion.identity);
            Destroy(fx, 2f);

            // Call level complete function
            LevelManager.main.LevelComplete();
        }
    }

    // Check if the disc is in the basket
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Basket") CheckWinState();
    }
     
    private void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Basket") CheckWinState();
    }

}


