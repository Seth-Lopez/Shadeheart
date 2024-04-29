using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class UIMenuMngrV2 : MonoBehaviour
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
    private bool hasOptions = false;
    private string currentNPC = "";
    private GameObject buttonsCanv;
    private bool hasSelectedNewQuest = false;
    private bool shouldReset = false;
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
            buttonsCanv.SetActive(true);
            if(hasOptions)
            {
                buttonsCanv.SetActive(true);
            }
            else
            {
                buttonsCanv.SetActive(false);
            }
        }
        else
        {
            movementTimer += Time.deltaTime * /*Speed*/ .5f;
            movementTimer = Mathf.Clamp(movementTimer, -0.7f, 0f);
            offset.m_Offset = new Vector2(0f, movementTimer);
            dialogueBox.transform.Find("Canvas").gameObject.SetActive(false);
            buttonsCanv.SetActive(false);
        }
    }

    private void setUp()
    {
        cam = GameObject.FindGameObjectWithTag("Camera");
        cineCam = cam.GetComponentInChildren<CinemachineVirtualCamera>();
        GameObject[] menus = GameObject.FindGameObjectsWithTag("Menus");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        questMngrV2 = FindObjectOfType<QuestMngrV2>().GetComponent<QuestMngrV2>();
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
                buttonsCanv = dialogueBox.transform.Find("CanvasOptions").gameObject;
                buttonsCanv.SetActive(true);
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
        return dialogueBox.gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }
    public void setHasOptions(bool value)
    {
        hasOptions = value;
    }
    public void setCurrentNPC(string value)
    {
        currentNPC = value;
    }
    public void setHasSelectedNewQuest(bool value)
    {   
        hasSelectedNewQuest = value;
    }
    public bool getHasSelectedNewQuest()
    {   
        return hasSelectedNewQuest;
    }
    public void setDialogueText(string value)
    {
        TextMeshProUGUI text = getDialogueText();
        text.text = value;
    }
    public void setShouldResetQuest(bool value)
    {   
        shouldReset = value;
    }
    public bool getShouldResetQuest()
    {   
        return shouldReset;
    }
    public void closeButtons()
    {   
        hasOptions = false;
        buttonsCanv.SetActive(false);
    }
}
