using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices.WindowsRuntime;
using System;


public class UIMenuMngr : MonoBehaviour
{
    private GameObject cam;
    private CinemachineVirtualCamera cineCam;
    [SerializeField] private GameObject playersName;
    [SerializeField] private GameObject characterIcons;
    [SerializeField] private GameObject headIcons;

    public bool openDialogueBox = false;
    private float movementTimer = 0;
    private List<Sprite> throwAwayAnims = new List<Sprite>();
    //Dialogue Box
    [SerializeField] private GameObject dialogueBox;
    // Quest TextBoxes
    [SerializeField] private GameObject questTitle;
    [SerializeField] private GameObject questsActive;
    [SerializeField] private GameObject questsCompleted;
    // Menus
    [SerializeField] private GameObject MainBackground;
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject Stats;
    [SerializeField] private GameObject Monsters;
    [SerializeField] private GameObject currentMenuOpen;
    [SerializeField] private bool isMenuOpen = false;
    [SerializeField] private bool isPauseMenuOpen = false;
    [SerializeField] private QuestMngrV2 questMngrV2;
    [SerializeField] private NPCMovement [] npcMov;
    [SerializeField] private GameObject currentNPC;
    public bool hasDialogueOptions = false;
    void Start()
    {
        setUp();
    }
    void openMenu(GameObject menu, GameObject close)
    {
        if(menu != close)
        {
            closeMenu(close);
            MainBackground.transform.Find("Canvas").gameObject.SetActive(true);
            menu.transform.Find("Canvas").gameObject.SetActive(true);
            currentMenuOpen = menu;
            isMenuOpen = true;
        }
        else
            closeMenu(close);
    }
    void closeMenu(GameObject menu)
    {
        if (menu != cam)
        {
            MainBackground.transform.Find("Canvas").gameObject.SetActive(false);
            menu.transform.Find("Canvas").gameObject.SetActive(false);
            currentMenuOpen = cam;
            isMenuOpen = false;
        }
    }
    void Update()
    {
        cameraMngr();
        checkForPauseMenu();
    }
    private void checkForPauseMenu()
    {
        if(!isPauseMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                openMenu(Stats, currentMenuOpen);
                setQuests();
            }
            if (Input.GetKeyDown(KeyCode.Q))
                openMenu(Inventory, currentMenuOpen);
            if (Input.GetKeyDown(KeyCode.M))
                openMenu(Monsters, currentMenuOpen);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            closeMenu(currentMenuOpen);
            isPauseMenuOpen = !isPauseMenuOpen;
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
            closeMenu(currentMenuOpen);
            dialogueBox.transform.Find("Canvas").gameObject.SetActive(true);
        }
        else
        {
            movementTimer += Time.deltaTime * /*Speed*/ .5f;
            movementTimer = Mathf.Clamp(movementTimer, -0.7f, 0f);
            offset.m_Offset = new Vector2(0f, movementTimer);
            dialogueBox.transform.Find("Canvas").gameObject.SetActive(false);
            dialogueBox.transform.Find("CanvasOptions").gameObject.SetActive(false);
        }
    }

    private void setUp()
    {
        cam = GameObject.FindGameObjectWithTag("Camera");
        cineCam = cam.GetComponentInChildren<CinemachineVirtualCamera>();
        GameObject[] menus = GameObject.FindGameObjectsWithTag("Menus");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        questMngrV2 = FindObjectOfType<QuestMngrV2>().GetComponent<QuestMngrV2>();
        npcMov = FindObjectsOfType<NPCMovement>();
        currentMenuOpen = cam;
        foreach (GameObject menu in menus)
        {
            if(menu.name == "Player's Name")
                playersName = menu;
            if(menu.name == "Charater Sprites")
                characterIcons = menu;
            if(menu.name == "Head Sprites")
                headIcons = menu;
            if(menu.name == "MainBackground")
                MainBackground = menu;
            if(menu.name == "Inventory")
                Inventory = menu;
            if(menu.name == "Stats")
                Stats = menu;
            if(menu.name == "Monsters")
                Monsters = menu;
            if(menu.name == "DialogueBox")
            {
                dialogueBox = menu;
                dialogueBox.transform.Find("CanvasOptions").gameObject.SetActive(false);
            }
                
        }
        if(player != null)
        {
            throwAwayAnims = player.GetComponent<UltAnimatorScript>().getThrowAwayAnims();
            playersName.gameObject.GetComponent<TextMeshProUGUI>().text = player.gameObject.name;
        }
        if(characterIcons != null)
        {
            Vector2 size = new Vector2(throwAwayAnims[3].texture.width  * throwAwayAnims[3].rect.width,throwAwayAnims[3].texture.height  * throwAwayAnims[3].rect.height);
            characterIcons.gameObject.GetComponent<Image>().sprite = throwAwayAnims[3];
            float headHeight = throwAwayAnims[3].rect.height / 2f * 1.37f;
            float newRectY = throwAwayAnims[3].rect.y + throwAwayAnims[3].rect.height - headHeight;
            Rect rect = new Rect(throwAwayAnims[3].rect.x, newRectY, throwAwayAnims[3].rect.width, headHeight);
            Sprite headSprite = Sprite.Create(throwAwayAnims[3].texture, rect, new Vector2(0.5f, 0.5f), throwAwayAnims[3].pixelsPerUnit);
            headIcons.gameObject.GetComponent<Image>().sprite = headSprite;
            headIcons.gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    
    //Sets Quests Menus in the player's Stats menu
    public void setQuests()
    {
        questMngrV2 = FindObjectOfType<QuestMngrV2>().GetComponent<QuestMngrV2>();
        questsActive.GetComponent<TextMeshProUGUI>().text = "Active Quests:\n";
        foreach (QuestMngrV2.Quest quest in questMngrV2.getActiveQuests())
        {
            questsActive.GetComponent<TextMeshProUGUI>().text += quest.title;
            questsActive.GetComponent<TextMeshProUGUI>().text += "\n - " + quest.playerDescription;
        }
        questsCompleted.GetComponent<TextMeshProUGUI>().text = "Completed Quests:\n";
        foreach (QuestMngrV2.Quest quest in questMngrV2.getCompletedQuests())
        {
            questsCompleted.GetComponent<TextMeshProUGUI>().text += quest.title;
            questsCompleted.GetComponent<TextMeshProUGUI>().text += "\n - " + quest.playerDescription;
        }
    }
    public bool getIsPauseMenuOpen()
    {
        return isPauseMenuOpen;
    }
    public bool getIsMenuOpen()
    {
        return isMenuOpen;
    }
    public void setIsPauseMenuOpen()
    {
        isPauseMenuOpen = false;
    }
    public void buttonPressing(string Menu)
    {
        if(Menu == "Inv")
            openMenu(Inventory, currentMenuOpen);
        if(Menu == "Stats")
            openMenu(Stats, currentMenuOpen);
        if(Menu == "Monsters")
            openMenu(Monsters, currentMenuOpen);
        if(Menu == "Discord")
            return;  //<<<<------------------------------------ PUT DISCORD LINK SERVER HERE!--------------------------------------
        if(Menu == "Pause")
        {
            closeMenu(currentMenuOpen);
            Pause pauseMgr = GameObject.Find("PauseMgr").GetComponent<Pause>();
            if(pauseMgr.getPaused())
            {
                pauseMgr.Resume();
                isPauseMenuOpen = false;
            }
            else
            {
                pauseMgr.PauseGame();
                isPauseMenuOpen = true;
            }
        }
    }
    public TextMeshProUGUI getDialogueText()
    {
        return dialogueBox.transform.Find("Canvas").gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void setDialogueText(string value)
    {
        TextMeshProUGUI text = getDialogueText();
        if(value == "" || hasDialogueOptions == false)
        {
            hasDialogueOptions = false;
            dialogueBox.transform.Find("CanvasOptions").gameObject.SetActive(false);
            text.text = value;
        }
        else if(hasDialogueOptions)
        {
            GameObject optCanv = dialogueBox.transform.Find("CanvasOptions").gameObject;
            optCanv.SetActive(true);
            text.text = value; 
            //currentNPC.GetComponent<NPCStats>().getNextLine()[0];
        }
    }
    public void setCurrentNPC(GameObject value){currentNPC = value;}
    public GameObject getCurrentNPC(){return currentNPC;}
    public void isTalking(bool value)
    {
        if(currentNPC != null)
        {
            foreach (NPCMovement npc in npcMov)
            {
                if(npc.gameObject.name == currentNPC.name)
                {
                    npc.setIsTalking(value);
                }
            }
        }
    }
    public void getNextLine(bool yesNo)
    {
        string line;
        (line, hasDialogueOptions) = currentNPC.GetComponent<NPCStats>().getNextLine()[0];
        if(yesNo)
        {
            setDialogueText(line.Split(new string[] { "^#" }, StringSplitOptions.RemoveEmptyEntries)[0]);
            GameObject.FindAnyObjectByType<DialogueMngr>().writeToQuestFile(currentNPC, 0);
            currentNPC.GetComponent<NPCStats>().retreiveDialogueOptions(true, 2);
            questMngrV2.setQuestsActiveComplete(currentNPC.name, true, false);
        }
        else
        {
            setDialogueText(line.Split(new string[] { "^#" }, StringSplitOptions.RemoveEmptyEntries)[1]);
        }

    }
}
