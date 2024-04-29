using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DialogueMngr : MonoBehaviour
{
    // The list that stores the dialogue data:
    private List<(string, List<string>, List<(string, bool, bool)>)> dialogueList = new List<(string, List<string>, List<(string, bool, bool)>)>();
    private List<(string, List<string>, List<(string, bool, bool)>)> QuestdialogueList = new List<(string, List<string>, List<(string, bool, bool)>)>();
    private string filePath = Path.Combine(Application.streamingAssetsPath, "Managers/DialogueOptions.txt");
    private string filePath2 = Path.Combine(Application.streamingAssetsPath, "Managers/QuestDialogueOptions.txt");
    private QuestMngrV2 queMng;
    void Awake()
    {
        dialogueList = ReadDialogueFromFile(filePath);
        QuestdialogueList = ReadDialogueFromFile(filePath2);
    }
    void Start()
    {
        queMng = GameObject.FindGameObjectWithTag("QuestMngrV2").GetComponent<QuestMngrV2>();
    }
    
    public void writeToQuestFile(GameObject npc, int counter = 0)
    {
        string[] lines = File.ReadAllLines(filePath2);
        int index = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("**" + npc.gameObject.name + "**"))
            {
                index = i;
            }
        }
        if (index != -1)
        {
            // Modify the next two lines
            lines[index + 1 + counter] = "1" + lines[index + 1 + counter].Substring(1);
            lines[index + 2 + counter] = "1" + lines[index + 2 + counter].Substring(1);
        }
        else
        {
            Debug.LogError("Title not found in file");
            return;
        }
        // Write the modified lines back to the file
        File.WriteAllLines(filePath2, lines);
        ReadDialogueFromFile(filePath2);
    }
    private List<(string, List<string>, List<(string, bool, bool)>)> ReadDialogueFromFile(string path)
    {
        List<(string, List<string>, List<(string, bool, bool)>)> dialogue = new List<(string, List<string>, List<(string, bool, bool)>)>();
        string[] lines = File.ReadAllLines(path);
        string currentTitle = "";
        List<string> nonQuestLines = new List<string>();
        List<(string, bool, bool)> questLines = new List<(string, bool, bool)>();
        bool hasBeenRead = false;
        bool hasOptions = false;
        string text = "";
        string[] parts;
        foreach (string line in lines)
        {
            // Check if the line starts with "**" indicating a title:
            if (line.StartsWith("**") && line.EndsWith("**"))
            {
                // If a title is already being processed, add it to the dialogue:
                if (!string.IsNullOrEmpty(currentTitle))
                {
                    dialogue.Add((currentTitle, nonQuestLines, questLines));
                }

                // Extract the title from the line:
                currentTitle = line.Substring(2, line.Length - 4);
                nonQuestLines = new List<string>();
                questLines = new List<(string, bool, bool)>();
            }
            else
            {
                if (line.Contains("_0_") || line.Contains("_1_"))
                {
                    parts = line.Split('_');

                    hasBeenRead = parts[0] == "1";
                    hasOptions = parts[1] == "1";
                    text = parts[2];
                    questLines.Add((text, hasOptions, hasBeenRead));
                }
                else if (line.Contains("_"))
                {
                    parts = line.Split('_');
                    hasBeenRead = parts[0] == "1";
                    hasOptions = false;
                    text = parts[1];
                    questLines.Add((text, hasOptions, hasBeenRead));
                }
                else
                {
                    nonQuestLines.Add(line);
                }
            }
        }
        // Add the last set of dialogue (if any) to the dialogue:
        if (!string.IsNullOrEmpty(currentTitle))
        {
            dialogue.Add((currentTitle, nonQuestLines, questLines));
        }
        return dialogue;
    }
    public List<(string, List<string>, List<(string, bool, bool)>)> getDialogueList(){return dialogueList;}
    public List<(string, List<string>, List<(string, bool, bool)>)> getQuestDialogueList(){return QuestdialogueList;}
}
