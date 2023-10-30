using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 5f;
    private float sprintSpeed;
    private float currentMovementSpeed;
    
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    void Start()
    {
            rb = GetComponent<Rigidbody2D>();
            currentMovementSpeed = walkingSpeed;
    }
    void updatingMovement()
    {
        
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        sprintSpeed = walkingSpeed + 5f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMovementSpeed = sprintSpeed;
        }
        else
        {
            currentMovementSpeed = walkingSpeed;
        }
    }
    void Update()
    {
        updatingMovement();
    }
    void FixedUpdate()
    {
        rb.velocity = movementDirection * currentMovementSpeed;
    }
}
