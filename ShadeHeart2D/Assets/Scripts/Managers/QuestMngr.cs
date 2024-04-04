using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
//Types of quests statuses Inactive -> need to find npc; Active -> player got quest; Complete -> player completed it
public enum QuestStatus
{
    Inactive,
    Active,
    Completed
}

[System.Serializable]
public class QuestMngr: MonoBehaviour
{
    private string questName;
    private string devDesc;
    private string questDesc;
    private int xpReward;
    private int[] rewardRarity;
    private int[] specificRewardId;
    private List<QuestMngr> quests = new List<QuestMngr>();
    private QuestStatus status;
    private static int numQuests = 0;
    private Dictionary<string, GameObject> menuDictionary = new Dictionary<string, GameObject>();
    private GameObject questTitle;
    private GameObject questInCompleted;
    private GameObject questCompleted;
    public QuestStatus Status
    {
        get { return status; }
    }

    void Start()
    {
        // All quests
        quests =  new List<QuestMngr>
        {
            new QuestMngr("Quest Name Here", 
                "Developer Quest Description Here", 
                "Player Quest Description Here", 
                100, //XP
                new int[]{1, 2, 3}, //RewardRarity
                new int[]{4, 5}), //RewardID
            // Add more quests here
                new QuestMngr("Chicken Finder", 
                "PLayer must find 5 chickens", 
                "Help old lady find 5 chickens", 
                100, //XP
                new int[]{1, 2, 3}, //RewardRarity
                new int[]{4, 5}), //RewardID
        };
        GameState GS = new GameState();
        GS.clearGameState(); 
        foreach (QuestMngr que in quests)
        {
            GS.addQuestToGameState(que.getQuestName());
        }
        GameObject[] menus = GameObject.FindGameObjectsWithTag("QuestMenus");
        foreach (GameObject menu in menus)
        {
            if(menu.name == "Target Quest")
                questTitle = menu;
            if(menu.name == "QinCom")
                questInCompleted = menu;
            if(menu.name == "QFin")
                questCompleted = menu;
        }
        List<QuestMngr> activeNotCompletedQuests = quests.Where(quest => quest.Status == QuestStatus.Active).ToList();
        if(questInCompleted != null)
        {
            int count = 0;
            foreach (QuestMngr quest in activeNotCompletedQuests)
            {
                count ++;
                questInCompleted.GetComponent<TextMeshProUGUI>().text += quest.questDesc;
            }
            Debug.Log("count: " + count);
        }
        else
            Debug.Log("BOo");
    }
    public QuestMngr(string name, string devDescription, string questDescription, int xp, int[] rarity, int[] specificIds)
    {
        questName = name;
        devDesc = devDescription;
        questDesc = questDescription;
        xpReward = xp;
        rewardRarity = rarity;
        specificRewardId = specificIds;
        status = QuestStatus.Inactive;
        numQuests++;
    }

    public void ActivateQuest(){status = QuestStatus.Active;}

    public void CompleteQuest()
    {
        status = QuestStatus.Completed;
        //give xp + rewards when that system is completed!
    }

    public string getQuestName()
    {
        return questName;
    }
    // Get all active quests
    public List<QuestMngr> GetActiveQuests()
    {
        return quests.FindAll(quest => quest.status == QuestStatus.Active);
    }
    // Get all completed quests
    public List<QuestMngr> GetCompletedQuests()
    {
        return quests.FindAll(quest => quest.status == QuestStatus.Completed);
    }
}


    
        

    

    