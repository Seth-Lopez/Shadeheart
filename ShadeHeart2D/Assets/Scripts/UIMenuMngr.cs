using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.UI;

public class UIMenuMngr : MonoBehaviour
{
    private GameObject cam;
    private CinemachineVirtualCamera cineCam;
    [SerializeField] private GameObject playersName;
    [SerializeField] private GameObject characterIcons;
    [SerializeField] private GameObject headIcons;
    public bool openDialogueBox = false;
    private float movementTimer = 0;
    private Dictionary<string, GameObject> menuDictionary = new Dictionary<string, GameObject>();
    private List<Sprite> throwAwayAnims = new List<Sprite>();

    
    // Quest Menus
    GameObject questTitle;
    GameObject questsActive;
    GameObject questsCompleted;

    [SerializeField] private QuestMngrV2 questMngrV2;

    void Start()
    {
        setUp();
        setQuests();
    }

    void Update()
    {
        cameraMngr();
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
        GameObject[] menus = GameObject.FindGameObjectsWithTag("Menus");
        GameObject[] QuestMenus = GameObject.FindGameObjectsWithTag("QuestMenus");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        questMngrV2 = FindObjectOfType<QuestMngrV2>().GetComponent<QuestMngrV2>();

        foreach (GameObject menu in menus)
        {
            menuDictionary[menu.name] = menu;
            if(menu.name == "Player's Name")
                playersName = menu;
            if(menu.name == "Charater Sprites")
                characterIcons = menu;
            if(menu.name == "Head Sprites")
                headIcons = menu;
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
        
        foreach (GameObject menu in QuestMenus)
        {
            if(menu.name == "QueTar")
                questTitle = menu;
            if(menu.name == "QueAct")
                questsActive = menu;
            if(menu.name == "QueCom")
                questsCompleted = menu;
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

    public void setMenuActive(string menuName)
    {
        foreach (var kvp in menuDictionary)
        {
            if (kvp.Key != menuName)
            {
                disableEnableCanvas(kvp.Value, false);
            }
        }

        if (menuDictionary.ContainsKey(menuName))
        {
            disableEnableCanvas(menuDictionary[menuName], true);
        }
    }

    public void SetMenuDeactive(string menuName)
    {
        if (menuDictionary.ContainsKey(menuName))
        {
            disableEnableCanvas(menuDictionary[menuName], false);
        }
    }
    private void disableEnableCanvas(GameObject parentGameObject, bool turnOn)
    {
        GameObject canvasGO = parentGameObject.transform.Find("Canvas").gameObject;
        if (canvasGO != null)
        {
            if(turnOn)
                canvasGO.SetActive(true);
            else
                canvasGO.SetActive(false);
        }
    }
}
