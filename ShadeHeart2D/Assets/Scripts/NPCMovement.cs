using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPCMovement : MonoBehaviour
{
    // For Movement: 
    [SerializeField] private float walkingSpeed = 1f;
    private bool walkPointSet = false;
    private Vector3 destPoint;
    private Vector2 movementDirection = new Vector2(0,0);
    private bool isMoving = false;
    private float rangePos = 2f;
    private float waitToMove = 5f;
    [SerializeField] private float timer = 0f;
    
    private float waitToMove2 = 5f;
    [SerializeField] private float timer2 = 0f;
    //NavMesh:
    UnityEngine.AI.NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }
    private void Update()
    {
        if(agent != null)
        {
            updatingMovement();
            updateIsMoving();
        }

    }
    private void updateIsMoving()
    {
        timer2 += Time.deltaTime;
        if(timer2 >= 2 && timer2 <= waitToMove2)
        {
            isMoving = false;
        }
        else if( timer2 >= waitToMove2)
        {
            timer2 = 0;
            isMoving = true;
        }
    }
    private void updatingMovement() 
    {
        timer += Time.deltaTime;
        if(timer >= waitToMove)
        {
            agent.speed = walkingSpeed;
            if(!walkPointSet)
            { 
                searchForDest();
            };
            if(walkPointSet) 
            {
                agent.SetDestination(destPoint);
            }
            if(Vector3.Distance(transform.position, destPoint) < 10f)
            {
                walkPointSet = false;
                timer = 0;
            }
        }
            
    }
    private void searchForDest()
    {
        float x = UnityEngine.Random.Range(-rangePos, rangePos);
        float y = UnityEngine.Random.Range(-rangePos, rangePos);
        destPoint = new Vector3(transform.position.x + x, transform.position.y + y, 0f);
        walkPointSet = true;
        movementDirection = new Vector2 (x,y);
        isMoving = false;
    }
    public Vector2 getMovDir(){ return movementDirection; }
    public bool getIsMoving(){ return !isMoving; }
}
