using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements.Experimental;

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

    private bool isTalking = false;
    private int debugCounter = 0;
    private int debugCounter2 = 0;
    int crntAnim = -1;
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
        if(!isTalking)
        {
            if(debugCounter == 1)
            {
                debugCounter = 0;
                timer = 0;
                timer2 = 0;
                debugCounter2 = 1;
            }
            if(agent != null)
            {
                updatingMovement();
                updateIsMoving();
            }
        }
        else
        {
            stopMoving();
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
    public void setIsTalking(bool value){ isTalking = value;}
    private void stopMoving()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(player.transform.position);
        movementDirection = new Vector2 ((player.transform.position - this.transform.position).normalized.x,(player.transform.position - this.transform.position).normalized.y);
        isMoving = false;
        agent.speed = 0;
        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
        {
            if (movementDirection.x > 0)//Right
            {
                if(crntAnim != 0 && crntAnim != 4)
                    debugCounter = 0;
                crntAnim = 0;
            }
            else//Left
            {
                if(crntAnim != 1 && crntAnim != 5)
                    debugCounter = 0;
                crntAnim = 1;
            }
        }
        else
        {
            if (movementDirection.y > 0)//Back
            {
                if(crntAnim != 2 && crntAnim != 6)
                    debugCounter = 0;
                crntAnim = 2;
            }
            else//Front
            {
                if(crntAnim != 3 && crntAnim != 7)
                    debugCounter = 0;
                crntAnim = 3;
            }
        }
        if(debugCounter == 0)
        {
            this.gameObject.GetComponent<UltAnimatorScript>().setCrntAnim(crntAnim);
            debugCounter = 1;
        }
    }
}
