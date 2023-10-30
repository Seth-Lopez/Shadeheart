using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using Unity.Mathematics;
public class PlayerScript : MonoBehaviour
{
    // For Movement: 
    [SerializeField] private float walkingSpeed = 5f;
    private float sprintSpeed;
    private float currentMovementSpeed;
    private Vector2 movementDirection;
    // RigidBody 2D:
    private Rigidbody2D rb;
    // For Health:
    private GameObject healthBarGameObject;
    private Image healthBar;
    [SerializeField] private float currentHealth = 100f;
    private float maxHealth = 100;
    // For Energy:
    private GameObject energyBarGameObject;
    private Image energyBar;
    [SerializeField] private float currentEnergy = 50;
    private float maxEnergy = 50;

    private void Start()
    {
        // Set RigidBody:
        rb = GetComponent<Rigidbody2D>();
        //Get Health Bar Component:
        healthBarGameObject = GameObject.FindWithTag("HealthBar");
        healthBar = healthBarGameObject.GetComponent<Image>();
        //Get Energy Bar Component:
        energyBarGameObject = GameObject.FindWithTag("EnergyBar");
        energyBar = energyBarGameObject.GetComponent<Image>();
        //Set Variables:
        currentMovementSpeed = walkingSpeed;
        currentHealth =  maxHealth;
        currentEnergy = maxEnergy;
    }

    private void Update()
    {
        updatingMovement();
        updatingHealthAndEnergy();
    }
    private void FixedUpdate()
    {
        rb.velocity = movementDirection * currentMovementSpeed;
    }

    // Updating Movement Speed: 
    public void setWalkingSpeed(float newWalkingSpeed){walkingSpeed = newWalkingSpeed;}
    public void setSprintSpeed(float newSprintSpeed){sprintSpeed = newSprintSpeed;}
    private void updatingMovement()
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
    private void updatingHealthAndEnergy()
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 100);
        energyBar.fillAmount = Mathf.Clamp(currentEnergy / maxEnergy, 0, 50);
    }
}
