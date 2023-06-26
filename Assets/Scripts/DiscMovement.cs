using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscMovement : MonoBehaviour
{
    private bool isMoving = false;
    private bool inHole;

    private Rigidbody2D rb2D;

    Vector2 firstClick;
    Vector2 secondClick;
    Vector2 firstAndSecond;

    float force;
    float degree;

    Vector3 mousePos;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize isMoving
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Check if the disc is moving
        if (rb2D.velocity.magnitude > 0)
        {
            // Set isMoving to true
            isMoving = true;
        }
        else
        {
            // Set isMoving to false
            isMoving = false;
        }

        // If the disc is not moving, allow the player to throw the disc
        if (!isMoving)
        {
            // When mouse is clicked, get the position of the mouse
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
                firstClick = new Vector2(mousePos.x, mousePos.y);
            }
            // When mouse is released, get the position of the mouse. Calculate force and direction of throw using the difference in mouse positioning
            if (Input.GetMouseButtonUp(0))
            {
                rb2D.velocity = Vector2.zero;
                mousePos = Input.mousePosition;
                secondClick = new Vector2(mousePos.x, mousePos.y);
                firstAndSecond = new Vector2(firstClick.x - secondClick.x, firstClick.y - secondClick.y);
                
                // Calculate Degree
                degree = (Mathf.Atan2(firstAndSecond.y, firstAndSecond.x) * Mathf.Rad2Deg);
                // Calculate Force
                force = Vector2.Distance(firstClick, secondClick) * 0.75f;
                
                // Using degree, create a direction vector
                Vector3 direction = Quaternion.AngleAxis(degree, Vector3.forward) * Vector3.right;
                // Apply force to the direction 
                rb2D.AddForce(direction * force);

                // Move the disc to the right
                float moveRight = 100f;
                rb2D.transform.position += new Vector3(moveRight * rb2D.velocity.magnitude, 0f, 0f);

                // Set isMoving to true after the disc has been thrown
                isMoving = true;
            }
        }


        // Control the speed at which the disc rotates
        float rotationMultiplier = 1000f;
        // Control the direction the disc spins. 1 for counter-clockwise, -1 for clockwise
        int rotationDirection = -1;
        // Rotate the disc while it is moving
        if (isMoving)
        {
            float rotationSpeed = rb2D.velocity.magnitude * rotationMultiplier;
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime * rotationDirection);
        }

        // Decelerate the disc at the end of its flight to prevent the player from waiting too long for it to come to a complete stop
        float deceleration = .1f;
        if (isMoving && rb2D.velocity.magnitude < deceleration)
        {
            rb2D.velocity = Vector2.zero;
        }
    }
}

