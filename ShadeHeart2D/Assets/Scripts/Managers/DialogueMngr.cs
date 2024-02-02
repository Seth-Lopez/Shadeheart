using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class DialogueMngr : MonoBehaviour
{
    // The list that stores the dialogue data:
    private List<(string, List<string>)> dialogueList = new List<(string, List<string>)>();
    private string filePath = Path.Combine(Application.dataPath, "Scripts/Managers/DialogueOptions.txt");

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
        List<string> currentLines = new List<string>();
        foreach (string line in lines)
        {
            // Check if the line starts with "**" indicating a title:
            if (line.StartsWith("**") && line.EndsWith("**"))
            {
                // If a title is already being processed, add it to the dialogueList:
                if (!string.IsNullOrEmpty(currentTitle))
                {
                    dialogueList.Add((currentTitle, currentLines));
                }

                // Extract the title from the line:
                currentTitle = line.Substring(2, line.Length - 4);
                currentLines = new List<string>();
            }
            else
            {
                // Add the line to the currentLines list:
                currentLines.Add(line);
            }
        }
        // Add the last set of dialogue (if any) to the dialogueList:
        if (!string.IsNullOrEmpty(currentTitle))
        {
            dialogueList.Add((currentTitle, currentLines));
        }
    }
    
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

    public List<(string, List<string>)> getDialogueList(){return dialogueList;}
}
