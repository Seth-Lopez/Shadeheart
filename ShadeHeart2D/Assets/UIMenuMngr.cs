using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UIMenuMngr : MonoBehaviour
{
    private GameObject cam;
    private CinemachineVirtualCamera cineCam;
    private GameObject[] menus;
    private GameObject dialogueBox;
    public bool openDialogueBox = false;
    private float movementTimer = 0;
    void Start()
    {
        setUp();
    }


    void Update()
    {
        cameraMngr();
        if(openDialogueBox)
        {
            dialogueBox.SetActive(true);
        }
        else
        {
            dialogueBox.SetActive(false);
        }
    }
    private void cameraMngr()
    {
        CinemachineCameraOffset offset = cineCam.GetComponent<CinemachineCameraOffset>();
        if(openDialogueBox)
        {
            movementTimer -= Time.deltaTime * /*Speed*/ 2f;
            movementTimer = Mathf.Clamp(movementTimer, -0.7f, 0f);
            offset.m_Offset = new Vector2(0f, movementTimer);
        }
        else
        {
            movementTimer += Time.deltaTime * /*Speed*/ .5f;
            movementTimer = Mathf.Clamp(movementTimer, -0.7f, 0f);
            offset.m_Offset = new Vector2(0f, movementTimer);
        }
    }
    private void setUp()
    {
        cam = GameObject.FindGameObjectWithTag("Camera");
        cineCam = cam.GetComponentInChildren<CinemachineVirtualCamera>();
        menus = GameObject.FindGameObjectsWithTag("Menus");
        foreach (GameObject s in menus) 
        {
            if(s.name == "DialogueBox")
            {
                dialogueBox = s;
            }
        }
    }
    public GameObject getDialogueBox()
    {
        return dialogueBox;
    }
}
