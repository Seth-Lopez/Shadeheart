using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DialogueMngr : MonoBehaviour
{
    // The list that stores the dialogue data:
    private List<(string, List<string>, List<(int, string, bool, string)>, List<(int, string, bool, string)>)> dialogueList = new List<(string, List<string>, List<(int, string, bool, string)>, List<(int, string, bool, string)>)>();
    private string filePath = Path.Combine(Application.streamingAssetsPath, "Managers/DialogueOptions.txt");
    private QuestMngrV2 queMng;
    void Awake()
    {
        ReadDialogueFromFile(filePath);
        // Debug Purposes:
        /*
        foreach ((string title, List<string> lines) in dialogueList)
        {
            Debug.Log("Title: " + title);
            foreach (string line in lines)
            {
                Debug.Log("Line: " + line);
            }
        }*/
    }
    //TEMPORARY -> use this as basis for other script
    void Start()
    {
        queMng = GameObject.FindGameObjectWithTag("QuestMngrV2").GetComponent<QuestMngrV2>();
       /* AddDialogueToFile
            (
                //Title:
                "newTitle", 
                //Lines of dialogue seperated by ","
                new List<string> 
                    { 
                        "New line 1", 
                        "New line 2" 
                    }
            );
        */
    }
    void ReadDialogueFromFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string currentTitle = "";
        List<string> nonQuestLines = new List<string>();
        List<(int number, string option, bool hasOptions, string text)> preQuestLines = new List<(int, string, bool, string)>();
        List<(int number, string option, bool hasOptions, string text)> postQuestLines = new List<(int, string, bool, string)>();
        foreach (string line in lines)
        {
            // Check if the line starts with "**" indicating a title:
            if (line.StartsWith("**") && line.EndsWith("**"))
            {
                // If a title is already being processed, add it to the dialogueList:
                if (!string.IsNullOrEmpty(currentTitle))
                {
                    dialogueList.Add((currentTitle, nonQuestLines, preQuestLines, postQuestLines));
                }

                // Extract the title from the line:
                currentTitle = line.Substring(2, line.Length - 4);
                nonQuestLines = new List<string>();
                preQuestLines = new List<(int, string, bool, string)>();
                postQuestLines = new List<(int, string, bool, string)>();
            }
            else
            {
                // Add the line to the currentLines list:
                if(!line.StartsWith("?") && !line.StartsWith("!"))
                {
                    nonQuestLines.Add(line);
                }
                if (line.StartsWith("?"))
                {
                    string[] parts = line.Split('_');
                    int number = int.Parse(parts[0].Substring(1)); // Remove the leading '?'
                    string option = parts.Length > 2 ? parts[1] : ""; // Check if there's an option
                    bool hasOptions = parts.Length > 2; // Check if there are options
                    string text = string.Join("_", parts.Skip(hasOptions ? 2 : 1)); // Join the remaining parts to get the text
                    preQuestLines.Add((number, option, hasOptions, text));
                }
                if (line.StartsWith("!"))
                {
                    string[] parts = line.Split('_');
                    int number = int.Parse(parts[0].Substring(1)); // Remove the leading '!'
                    string text = string.Join("_", parts.Skip(1)); // Join the remaining parts to get the text
                    postQuestLines.Add((number, "", false, text)); // Use 0 for number, "" for option, and false for hasOptions
                }

            }
        }
        // Add the last set of dialogue (if any) to the dialogueList:
        if (!string.IsNullOrEmpty(currentTitle))
        {
            dialogueList.Add((currentTitle, nonQuestLines, preQuestLines, postQuestLines));
        }
    }
/* 
    void AddDialogueToFile(string title, List<string> lines)
    {
        string existingContent = File.ReadAllText(filePath);
        // Add title with format **title**
        string newDialogue = $"\n**{title}**";
        foreach (string line in lines)
        {
            newDialogue += $"\n{line}";
        }
        // Append new lines
        string updatedContent = existingContent + newDialogue;
        // Add to variable
        dialogueList.Add((title, lines));
        // Write to file
        File.WriteAllText(filePath, updatedContent);
    }
*/ 
    public List<(string, List<string>, List<(int, string, bool, string)>, List<(int, string, bool, string)>)> getDialogueList(){return dialogueList;}
}
