using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Extremely basic movement script for player
public class playerMovement : MonoBehaviour
{
    //Global Variables for movement
    private float currentMovementSpeed = 5f;
    private Vector2 movementDirection;
    private Rigidbody2D rb; 

    //Set variables
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
    }
    // Check for input
    private void Update()
    {
        updatingMovement();
    }
    //Update Movement
    private void FixedUpdate()
    {
        rb.velocity = movementDirection * currentMovementSpeed;
    }
    // Input Management
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
