using System.Collections;
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
    void Start()
    {
        setUp();
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
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
            if(playersName != null)
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
