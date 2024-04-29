using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

//Complex Class for movement within Overworld
public class EnemyBehavior : MonoBehaviour
{
    // For Movement: 
    [SerializeField] private float walkingSpeed = 1f;
    private float sprintSpeed = 3f;
    private bool walkPointSet = false;
    private Vector3 destPoint;
    private Vector3 previousPosition;
    [SerializeField] private float rangePos = 3f;
    [SerializeField] private float waitToMove = 5f;
    [SerializeField] private float timer = 0f;
    //NavMesh:
    [SerializeField] LayerMask groundLayer;
    UnityEngine.AI.NavMeshAgent agent;
    // RigidBody 2D:
    private Rigidbody2D rb;
    // Line Of Sight
    private GameObject player;
    private bool hasLOS = false;
    [SerializeField] private float LOSDist;
    public int enemyID;

    //For loading into battle scene
    public int battleLocation = 0;
    public SceneLoader loader;

    private void Awake()
    {
        loader = GameObject.FindAnyObjectByType<SceneLoader>();
    }
    private void Start()
    {
        //Set RigidBody:
        rb = GetComponent<Rigidbody2D>();
        //Set Player Object:
        player = GameObject.FindGameObjectWithTag("Player");
        previousPosition = transform.position;
        //Set NavMesh
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    // Check Input
    private void Update()
    {
        updatingMovement();
    }
    // Check for Line Of Sight
    private void FixedUpdate()
    {
        
        lineOfSight();
    }
    // Checks if they have line of sight they move towards the player
    private void updatingMovement() 
    { 
        float x = Mathf.Abs(player.transform.position.x - transform.position.x);
        float y = Mathf.Abs(player.transform.position.y - transform.position.y);
        timer += Time.deltaTime;
        if(hasLOS && x <= LOSDist && y <= LOSDist)
        {
            agent.speed = sprintSpeed;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            if(timer >= waitToMove)
            {
                agent.speed = sprintSpeed;
                if(!walkPointSet) searchForDest();
                if(walkPointSet) {agent.SetDestination(destPoint); }
                if(Vector3.Distance(transform.position, destPoint) < 10f)
                {
                    walkPointSet = false;
                    timer = 0;
                }
            }
        }
        
    }
    // Setters
    public void setWalkingSpeed(float newWalkingSpeed) { walkingSpeed = newWalkingSpeed; }
    public void setSprintSpeed(float newSprintSpeed) { sprintSpeed = newSprintSpeed; }
    public void setLineOfSightDistance(float newLOSDist) { LOSDist = newLOSDist; }
    // Raycast for line of sight
    private void lineOfSight()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if(ray.collider != null) hasLOS = ray.collider.CompareTag("Player");
    }
    //Checks for a valid place to move within World.
    private void searchForDest()
    {
        float x = UnityEngine.Random.Range(-rangePos, rangePos);
        float y = UnityEngine.Random.Range(-rangePos, rangePos);
        destPoint = new Vector3(transform.position.x + x, transform.position.y + y, 0f);
        walkPointSet = true;
    }
    // Recursively set the opacity to 0 for the specified Transform and its children
    void SetOpacityToZeroRecursive(Transform parent)
    {
        SpriteRenderer renderer = parent.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            Color color = renderer.color;
            color.a = 0f; // Set alpha (opacity) to 0
            renderer.color = color;
        }
        for (int i = 0; i < parent.childCount; i++) SetOpacityToZeroRecursive(parent.GetChild(i));       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Replace "fadeToBlack" with the name of your scene
            //UnityEngine.SceneManagement.SceneManager.LoadScene("fadeToBlack");
            
            PlayerPrefs.SetInt("battleLocation", battleLocation);
            PlayerPrefs.SetInt("enemyID", enemyID);
            string sceneLoadedFrom = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetString("sceneLoadedFrom", sceneLoadedFrom);
            loader.LoadBattle("Battle");
            this.gameObject.SetActive(false);
            //SceneManager.LoadScene("Battle");
        }
    }
}

