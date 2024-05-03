using UnityEngine;
using Cinemachine;
using EasyTransition;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

public class IntroScript : MonoBehaviour
{
    [SerializeField] public float speed = 3.0f;
    [SerializeField] private float timer = 0.0f;
    [SerializeField] float duration = 3;
    private bool slowingDown = false;
    [SerializeField] GameObject[] wheels;
    private GameObject rBus;
    private GameObject dBus;
    private GameObject lBus;
    private GameObject blinker;
    int phase = 1;
    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private float blinkInterval = 0.5f; // Blink interval in seconds
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    [SerializeField] private TransitionSettings trans;
    [SerializeField] private GameObject player;
    private bool timeStarts = false;
    private bool eventTriggered = false;
    void Awake()
    {
        rBus = GameObject.Find("RightBus");
        dBus = GameObject.Find("DownBus");
        lBus = GameObject.Find("LeftBus");
        SetAlphaRecursively(dBus.transform, 0);
        SetAlphaRecursively(lBus.transform, 0);
        trans.transitionSpeed = .4f;
    }
    int counter = 0;
    void Update()
    {
        if(PlayerPrefs.GetInt("IntroPlayed") == 1)
        {
            print("Hi");
            intro();
        }
    }
    private void intro()
    {
        //******************************--- PHASE 1 ---*******************************************
        if(phase == 1)
        {
            if (timer <= 11.6f)
            {
                if (transform.position.x <= -81.26f && !slowingDown)
                {
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                }
                else
                {
                    slowingDown = true;
                    if (speed < 0.01f)
                    {
                        speed = 0f;
                    }
                    else
                    {
                        speed -= Time.deltaTime * speed; // Decelerate smoothly
                    }
                    transform.Translate(Vector3.right * speed * Time.deltaTime);
                    
                    foreach(GameObject wheel in wheels)
                    {
                        wheel.gameObject.GetComponent<Wheels>().isSlowingDown = true;
                    }
                    if(counter == 0)
                    {
                        TransitionManager.Instance().Transition(trans, 4.5f);
                        counter+=1;
                    }
                }
                if(speed == 0)
                {
                    //******************************--- PHASE 2 ---*******************************************
                    phase = 2;
                    // ADD TRANSITION HERE!!
                    SetAlphaRecursively(rBus.transform, 0);
                    SetAlphaRecursively(dBus.transform, 100);
                    transform.position = new Vector3(-65, 44.8f, 0);
                    slowingDown = false;
                    speed = 10f;
                }
            }
            else{timer += Time.deltaTime;}
        }
        if(phase == 2)
        {
            if (timer <= duration)
            {
                movingDown();
            }
            else{timer += Time.deltaTime;}
            if(slowingDown)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= blinkInterval)
                {
                    elapsedTime = 0;
                    if (blinker != null)
                    {
                        Color color = blinker.GetComponent<SpriteRenderer>().color;
                        color.a = color.a == 0f ? 100f : 0f; // Toggle between 0 and 1
                        blinker.GetComponent<SpriteRenderer>().color = color;
                    }
                }

                if(counter == 1)
                {
                    TransitionManager.Instance().Transition(trans, 4.5f);
                    counter+=1;
                }
            }
            if(speed == 0)
            {
                //******************************--- PHASE 3 ---*******************************************
                phase = 3;
                // ADD TRANSITION HERE!!
                SetAlphaRecursively(dBus.transform, 0);
                SetAlphaRecursively(rBus.transform, 100);
                slowingDown = false;
                speed = 10f;
                transform.position = new Vector3(-54.26f, -47.56f, 0);
            }
        }
        if(phase == 3)
        {
            if (timer <= duration)
            {
                movingRight2();
            }
            else{timer += Time.deltaTime;}
            if(speed == 0)
            {
                //******************************--- PHASE 4 ---*******************************************
                phase = 4;
                
                if(counter == 2 && slowingDown)
                {
                    Debug.Log("H");
                    trans.transitionSpeed = 1;
                    TransitionManager.Instance().Transition(trans, 0f);
                    timeStarts = true;
                    Debug.Log("H");
                    counter+=1;
                }
            }
        }
        if(phase == 4)
        {
            //ADD TRANSITION HERE
             timer += Time.deltaTime;
            if (timer >= 1f && !eventTriggered)
            {
                eventTriggered = true;
                if(counter == 3)
                {
                    SetAlphaRecursively(rBus.transform, 0);
                    Color color = player.GetComponent<SpriteRenderer>().color;
                    color.a = 100;
                    player.GetComponent<SpriteRenderer>().color = color;
                    cinemachine.Follow = player.transform;
                    cinemachine.LookAt = player.transform;
                    counter += 1;
                    PlayerPrefs.SetInt("IntroPlayed", 2);
                }
            }
            
        }
    }

     void SetAlphaRecursively(Transform parent, float alpha)
    {
        foreach (Transform child in parent)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if(child.gameObject.name == "Blinker")
            {
                blinker = child.gameObject; 
            }
            if (spriteRenderer != null)
            {
                if(child.gameObject.name != "Blinker" || alpha == 0)
                {
                    Color color = spriteRenderer.color;
                    color.a = alpha;
                    spriteRenderer.color = color;
                }
            }
            if (child.childCount > 0)
                SetAlphaRecursively(child, alpha);
        }
    }
    void movingDown()
    {
        if (transform.position.y >= -27.5 && !slowingDown)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        else
        {
            slowingDown = true;
            if (speed < 0.01f)
            {
                speed = 0f;
            }
            else
            {
                speed -= Time.deltaTime * speed; // Decelerate smoothly
            }
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
    void movingRight2()
    {
        if (transform.position.x < -53.2 && !slowingDown)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            slowingDown = true;
            if (speed < 0.01f)
            {
                speed = 0f;
            }
            else
            {
                speed -= Time.deltaTime * speed; // Decelerate smoothly
            }
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
