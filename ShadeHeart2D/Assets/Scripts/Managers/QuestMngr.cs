using System.Collections.Generic;
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
    private List<QuestMngr> quests;
    private QuestStatus status;

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
        };
        GameState GS = new GameState();
        GS.clearGameState(); 
        foreach (QuestMngr que in quests)
        {
            GS.addQuestToGameState(que.getQuestName());
        }
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


    
        

    

    