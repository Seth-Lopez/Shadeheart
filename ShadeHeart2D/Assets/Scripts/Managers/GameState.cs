using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
//This class allows other scripts to see what Quests are completed & what Gamestate the quest is currently at...
//We can add quests (not remove) to a txt file by using the function "addQuestToGameState" to remove or add more we must add that feature
public class GameState : MonoBehaviour
{
    //File Path
    private string filePath = Path.Combine(Application.dataPath, "Scripts/Managers/GameState.txt");
    //Allows all other scripts to know what quests have been completed
    List<(int, bool)> GameStateVar = new List<(int, bool)>();
    //Allows Modders / Us to know what name a given quest id in the GameState.txt 
    List<string> QuestName = new List<string>();
    //Allows Modders / Us to know what number a given quest name is in the GameState.txt
    List<int> QuestIDNumber = new List<int>();
    // When game loads it checks the .txt and sets states
    void Awake()
    {
        bool firstLine = true;
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                // First line contains Quest Names -> sets QuestName && QuestIDNumber
                if(firstLine)
                {
                    string[] stringArray = line.Split(',');
                    int questId = 0;
                    foreach (string name in stringArray)
                    {
                        QuestName.Add(name);
                        QuestIDNumber.Add(questId);
                    }
                    questId+=1;
                    firstLine = false;
                }
                //All other lines besides the first are the quest's state -> sets GameStateVar
                else
                {
                    string[] parts = line.Split(':');
                    if (int.TryParse(parts[0], out int questNumber) && bool.TryParse(parts[1], out bool questState))
                    {
                        GameStateVar.Add((questNumber, questState));
                    }
                }
                
            }
        }
    }
    void Start()
    {
    }
    public void clearGameState()
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Truncate)){}
    }
    public void addQuestToGameState(string questName)
    {
        //Adds quest's name
        QuestName.Add(questName);
        //Adds quest's id
        QuestIDNumber.Add(QuestIDNumber.Count);
        //Add gamestate
        GameStateVar.Add((QuestIDNumber.Count, false));
        //Builds text file
        StringBuilder contentBuilder = new StringBuilder();
        foreach (string quest in QuestName)
        {
            contentBuilder.Append(quest);
            contentBuilder.Append(",");
        }
        if (contentBuilder.Length > 0)
        {
            contentBuilder.Length -= 1;
        }
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine(contentBuilder);
        }
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            foreach ((int intValue, bool boolValue) in GameStateVar)
            {
                writer.WriteLine($"{intValue}:{boolValue}");
            }
        }
    }
    /*
    public string getQuestNameById(int id)
    {
        if(id <= QuestIDNumber.Count - 1)
            return QuestName[id]; 
        return "Quest Id Does Not Exist.";
    }
    public int getQuestIDByName(string name)
    {
        int count = 0;
        foreach(string questname in QuestName)
        {
            if(name == questname)
            {
                return count;
            }
            count +=1;
        }
        return -1;
    }*/
    public List<(int, bool)> getGameStateVar()
    {
        return GameStateVar;
    }
    public List<string> getQuestNames()
    {
        return QuestName;
    }
}
