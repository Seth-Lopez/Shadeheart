using System.Collections.Generic;
using UnityEngine;


public class UltAnimatorScript : MonoBehaviour
{
    [SerializeField] private Texture2D SpriteSheet;
    private const string spriteFolder = "CharacterSheets";
    private Dictionary<string, List<Sprite>> animationDict;
    [SerializeField] private PlayerScript player;
    [SerializeField] private NPCMovement NPC;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float frameRate = .1f;
    private float frames = .1f;
    private int count = 0;
    private int crntAnim = 7;

    void Start()
    {
        setUp();
    }
    
    void Update()
    {
        Conditions();
    }
    // Sets Variables && animation sheet
    private void setUp()
    {
        spriteRenderer = transform.gameObject.GetComponent<SpriteRenderer>();
        Dictionary<string, List<Sprite>> spriteSheet = LoadSprites();
        if(transform.name == "Player"){player = transform.gameObject.GetComponent<PlayerScript>();} else {NPC = transform.gameObject.GetComponent<NPCMovement>();}
        foreach (var spriteList in spriteSheet)
        {
            if (spriteList.Key == SpriteSheet.name)
            {
                //Debug.Log("Sprite Sheet: " + spriteList.Key + ", Count: " + spriteList.Value.Count);
                animationDict = setAnimationLists(spriteList.Value);
            }
        }
    }
    //Plays Animation for character
    private void PlayAnimation(List<Sprite> sprites)
    {
        frames -= Time.deltaTime;
        if(frames < 0)
        {
            spriteRenderer.sprite = sprites[count];
            count++;
            frames = frameRate;
        }
    }
    // Player Movement Controls
    private void Conditions()
    {
        Vector2 movementDirection;
        bool NotMoving;

        if(count == 6)
            count = 0;
        if(transform.name == "Player")
        {
            movementDirection = player.getMovDir();
            NotMoving = player.getIsMoving();
        }
        else
        {
            movementDirection = NPC.getMovDir();
            NotMoving = NPC.getIsMoving();
        }
        movementDirection.Normalize();

        if(!NotMoving)
        {
            if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y))
            {
                if (movementDirection.x > 0)//Right
                {
                    if(crntAnim != 0){crntAnim = 0; frames = -100; count = 0;}
                    PlayAnimation(animationDict["WalkingRight"]);
                }
                else//Left
                {
                    if(crntAnim != 1){crntAnim = 1; frames = -100; count = 0;}
                    PlayAnimation(animationDict["WalkingLeft"]);
                }
            }
            else
            {
                if (movementDirection.y > 0)//Back
                {
                    if(crntAnim != 2){crntAnim = 2; frames = -100; count = 0;}
                    PlayAnimation(animationDict["WalkingBack"]);
                }
                else//Front
                {
                    if(crntAnim != 3){crntAnim = 3; frames = -100; count = 0;}
                    PlayAnimation(animationDict["WalkingForward"]);
                }
            }
        }
        else
        {
            if (crntAnim == 0 || crntAnim == 4)//Right
            {
                if(crntAnim != 4){crntAnim = 4; frames = -100; count = 0;}
                PlayAnimation(animationDict["IdleRight"]);
            }
            else if(crntAnim == 1 || crntAnim == 5)//Left
            {
                if(crntAnim != 5){crntAnim = 5; frames = -100; count = 0;}
                PlayAnimation(animationDict["IdleLeft"]);
            }
            else if (crntAnim == 2  || crntAnim == 6)//Back
            { 
                if(crntAnim != 6){crntAnim = 6; frames = -100; count = 0;}
                PlayAnimation(animationDict["IdleBack"]);
            }
            else if (crntAnim == 3 || crntAnim == 7)//Front
            {
                if(crntAnim != 7){crntAnim = 7; frames = -100; count = 0;}
                PlayAnimation(animationDict["IdleForward"]);
            }
            
        }
        //if Sitting: 3
        /*
        animationDict.Add("SittingRight", new List<Sprite>());      9
        animationDict.Add("SittingLeft", new List<Sprite>());       10
        */
    }
    // Setting Animations : 
    public static Dictionary<string, List<Sprite>> LoadSprites()
    {
        Dictionary<string, List<Sprite>> spriteLists = new Dictionary<string, List<Sprite>>();
        Sprite[] allSprites = Resources.LoadAll<Sprite>(spriteFolder);
        foreach (Sprite sprite in allSprites)
        {
            string spriteSheetName = sprite.texture.name;
            if (!spriteLists.ContainsKey(spriteSheetName))
            {
                spriteLists[spriteSheetName] = new List<Sprite>();
            }
            spriteLists[spriteSheetName].Add(sprite);
        }
        return spriteLists;
    }

    private Dictionary<string, List<Sprite>> setAnimationLists(List<Sprite> sprites)
    {
        Dictionary<string, List<Sprite>> animationDict = new Dictionary<string, List<Sprite>>();
        animationDict.Add("ThrowAways", new List<Sprite>());
        animationDict.Add("IdleRight", new List<Sprite>());
        animationDict.Add("IdleBack", new List<Sprite>());
        animationDict.Add("IdleLeft", new List<Sprite>());
        animationDict.Add("IdleForward", new List<Sprite>());
        animationDict.Add("WalkingRight", new List<Sprite>());
        animationDict.Add("WalkingBack", new List<Sprite>());
        animationDict.Add("WalkingLeft", new List<Sprite>());
        animationDict.Add("WalkingForward", new List<Sprite>());
        animationDict.Add("SittingRight", new List<Sprite>());
        animationDict.Add("SittingLeft", new List<Sprite>());

        foreach (var sprite in sprites)
        {
            int index = sprites.IndexOf(sprite);
            if (index >= 0 && index <= 3)
            {
                animationDict["ThrowAways"].Add(sprite);
            }
            else if (index >= 4 && index <= 9)
            {
                animationDict["IdleRight"].Add(sprite);
            }
            else if (index >= 10 && index <= 15)
            {
                animationDict["IdleBack"].Add(sprite);
            }
            else if (index >= 16 && index <= 21)
            {
                animationDict["IdleLeft"].Add(sprite);
            }
            else if (index >= 22 && index <= 27)
            {
                animationDict["IdleForward"].Add(sprite);
            }
            else if (index >= 28 && index <= 33)
            {
                animationDict["WalkingRight"].Add(sprite);
            }
            else if (index >= 34 && index <= 39)
            {
                animationDict["WalkingBack"].Add(sprite);
            }
            else if (index >= 40 && index <= 45)
            {
                animationDict["WalkingLeft"].Add(sprite);
            }
            else if (index >= 46 && index <= 51)
            {
                animationDict["WalkingForward"].Add(sprite);
            }
            else if (index >= 63 && index <= 68)
            {
                animationDict["SittingRight"].Add(sprite);
            }
            else if (index >= 69 && index <= 74)
            {
                animationDict["SittingLeft"].Add(sprite);
            }
        }
        return animationDict;
    }
    public List<Sprite> getThrowAwayAnims()
    {
        setUp();
        return animationDict["ThrowAways"];
    }
    public void setCrntAnim(int value)
    {
        crntAnim = value;
    }
}
