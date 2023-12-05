using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float currentMovementSpeed = 5f;
    private Vector2 movementDirection;
    Rigidbody2D rb; 

    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        updatingMovement();
    }
    private void FixedUpdate()
    {
        rb.velocity = movementDirection * currentMovementSpeed;
    }

    private void updatingMovement()
    {
        
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMovementSpeed = 10;
        }
        else
        {
            currentMovementSpeed = 5;
        }
    }
}
