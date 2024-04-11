using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class QuestMngrV2 : MonoBehaviour
{
    private string filePath = Path.Combine(Application.dataPath, "Scripts/Managers/GameState.txt");
    private List<Quest> quests = new List<Quest>();

    public class Quest
    {
        public string title;
        public bool isActive;
        public bool isCompleted;
        public string developerDescription;
        public string playerDescription;
        public int xpValue;
    }

    void Awake()
    {
        setQuests();
    }

    // Reads from file and then sets the List<Quest> quests variable 
    private void setQuests()
    {
        string[] lines = File.ReadAllLines(filePath);
        Quest quest = new Quest();
        foreach (string line in lines)
        {
            if (line.StartsWith("**"))
                quest = new Quest
                {
                    title = line.Replace("**", "").Trim()
                };
            else
            {
                string[] parts = line.Split(':');
                int lineNumber = int.Parse(parts[0]);
                string value = parts[1].Trim();
                switch (lineNumber)
                {
                    case 1:
                        quest.isActive = bool.Parse(value);
                        break;
                    case 2:
                        quest.isCompleted = bool.Parse(value);
                        break;
                    case 3:
                        quest.developerDescription = value;
                        break;
                    case 4:
                        quest.playerDescription = value;
                        break;
                    case 5:
                        quest.xpValue = int.Parse(value);
                        break;
                }
                if(lineNumber == 5)
                {
                    quests.Add(quest);
                }
            }
        }
    }
    // adds infinite new quest every time you run the game (Does not need to be removed)
    private void addNewQuest()
    {
        List<Quest> newQuests = new List<Quest>
        {
            new Quest
            {
                title = "",
                isActive = false,
                isCompleted = false,
                developerDescription = "",
                playerDescription = "",
                xpValue = 100
            },
            new Quest
            {
                title = "",
                isActive = false,
                isCompleted = false,
                developerDescription = "",
                playerDescription = "",
                xpValue = 100
            }
        };

        foreach(Quest newQuest in newQuests)
        {
            if(newQuest.title != "" && !quests.Any(q => q.title == newQuest.title))
            {
                quests.Add(newQuest);
            }
        }
        rewriteToFile();
    }
    // Rewrites all quests to the file in the proper format
    private void rewriteToFile()
    {
        File.WriteAllText(filePath, string.Empty);
        foreach (Quest quest in quests)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"**{quest.title}**");
                writer.WriteLine($"1:{quest.isActive}");
                writer.WriteLine($"2:{quest.isCompleted}");
                writer.WriteLine($"3:{quest.developerDescription}");
                writer.WriteLine($"4:{quest.playerDescription}");
                writer.WriteLine($"5:{quest.xpValue}");
            }
        }
    }
    // Debugging
    private void debugCheckPurposes()
    {
        foreach (Quest quest in quests)
        {
            Debug.Log("Title: " + quest.title);
            Debug.Log("IsActive: " + quest.isActive);
            Debug.Log("IsCompleted: " + quest.isCompleted);
            Debug.Log("DeveloperDescription: " + quest.developerDescription);
            Debug.Log("PlayerDescription: " + quest.playerDescription);
            Debug.Log("XPValue: " + quest.xpValue);
            Debug.Log("----------------------");
        }
    }
    // Public Functions:
    public List<Quest> getActiveQuests()
    {
        return quests.Where(quest => quest.isActive).ToList();
    }
    public List<Quest> getCompletedQuests()
    {
        return quests.Where(quest => quest.isCompleted).ToList();
    }
}
