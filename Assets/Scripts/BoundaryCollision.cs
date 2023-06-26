using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryCollision : MonoBehaviour
{

    // The force to apply to the disc when it collides with the boundary
    public float bounceForce = 5f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the disc
        if (collision.gameObject.CompareTag("Disc"))
        {
            // Get the rigidbody component of the disc
            Rigidbody2D discRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

            // Calculate the bounce direction
            Vector2 bounceDirection = (collision.GetContact(0).point - (Vector2)transform.position).normalized;

            // Apply the bounce force to the disc
            discRigidbody.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
