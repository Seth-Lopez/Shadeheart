using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
public class QuestMngrV2 : MonoBehaviour
{
    private string filePath = Path.Combine(Application.streamingAssetsPath, "Managers/GameState.txt");
    private List<Quest> quests = new List<Quest>();
    [SerializeField] private GameObject queObj;
    private int spawnOne = 0;
    public class Quest
    {
        public string title;
        public bool isActive;
        public bool isCompleted;
        public string developerDescription;
        public string playerDescription;
        public int xpValue;
        public string npcID;
    }

    void Awake()
    {
        setQuests();
        addNewQuest();
    }
    void Update() 
    {
        foreach(Quest que in quests)
        {
            if(que.isActive && spawnOne == 0)
            {
                spawnOne += 1;
                Instantiate(queObj);
            }
        }
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
                    case 6:
                        if (value != "")
                        {
                            if (value.StartsWith("!"))
                                quest.npcID = value.Substring(1);
                            else
                                quest.npcID = value;
                        }
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
                title = "Park Shade",
                isActive = false,
                isCompleted = false,
                developerDescription = "Investigate reports of strange shadows in the city park and confront the Shadewalker causing the disturbances. This quest introduces players to the battle system and hints at a deeper mystery involving what attracts Shadewalkers to certain elements in the city.",
                playerDescription = "Reports of eerie shadows in the city park have alarmed the citizens. Investigate the disturbances and confront whatever is causing them.",
                xpValue = 150,
                npcID = "!oldlady"
            },
            new Quest
            {
                title = "Whisp House",
                isActive = false,
                isCompleted = false,
                developerDescription = "Players investigate unexplained phenomena in one of the houses reported by Jane, leading to a confrontation with a Shadewalker. Players discover an item attracting the Shadewalkers, hinting at a larger mystery.",
                playerDescription = "Jane has reported strange occurrences in her house. Choose a house to investigate thoroughly and uncover the source of these phenomena.",
                xpValue = 200,
                npcID = ""
            },
            new Quest
            {
                title = "Seal Shade",
                isActive = false,
                isCompleted = false,
                developerDescription = "With the newfound knowledge of what's attracting Shadewalkers, players must find a way to remove or neutralize the problematic item in the shop area. The quest culminates in a final confrontation, with outcomes affecting future gameplay expansions.",
                playerDescription = "Use your knowledge about what attracts Shadewalkers to remove or neutralize the source of disturbances in the shop area. Your actions here could shape the future relationship with the Shadewalkers.",
                xpValue = 250,
                npcID = ""
            },
            new Quest
            {
                title = "Lost Dog",
                isActive = false,
                isCompleted = false,
                developerDescription = "A lost dog in the park turns out to be a minor Shadewalker. Players can choose to battle or help it return to the shadow realm, introducing moral choices.",
                playerDescription = "A distressed dog in the park seems out of this world. Determine if it's friend or foe, and decide its fate.",
                xpValue = 75,
                npcID = ""
            },
            new Quest
            {
                title = "School Spirit",
                isActive = false,
                isCompleted = false,
                developerDescription = "Investigate a 'haunted' classroom at the local school. Players find a small, scared Shadewalker, leading to a peaceful resolution or further chaos based on player actions.",
                playerDescription = "The school is in uproar over a haunted classroom. Investigate and soothe the students' and the creature's fears.",
                xpValue = 100,
                npcID = ""
            },
            new Quest
            {
                title = "",
                isActive = false,
                isCompleted = false,
                developerDescription = "",
                playerDescription = "",
                xpValue = 100,
                npcID = ""
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
                writer.WriteLine($"6:{quest.npcID}");
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
            Debug.Log("NPCID: " + quest.npcID);
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
    public List<string> getAllNpcIds()
    {
        return quests.Select(quest => quest.npcID).ToList();
    }
    public void setQuestsActiveComplete(string npcName, bool active, bool complete)
    {
        foreach (Quest quest in quests)
        {
            if (quest.npcID == npcName)
            {
                if(quest.isActive && complete)
                {
                    quest.isActive = false;
                    quest.isCompleted = true;
                }
                else if(quest.isActive == false && active)
                {
                    quest.isActive = true;
                }
            }
        }
        rewriteToFile();
    }
    public List<Quest> getQuests(){return quests;}
}
