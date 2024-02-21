using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class InventoryMngr : MonoBehaviour
{
    // The list that stores the Items data:
    private List<(string, List<string>)> ItemsList = new List<(string, List<string>)>();
    private string filePath = Path.Combine(Application.dataPath, "Scripts/Managers/InventoryItems.txt");

    void Awake()
    {
        ReadItemsFromFile(filePath);
        // Debug Purposes:
        /*
        foreach ((string title, List<string> lines) in ItemsList)
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
       /* AddItemsToFile
            (
                //Title:
                "newTitle", 
                //Lines of Items seperated by ","
                new List<string> 
                    { 
                        "New line 1", 
                        "New line 2" 
                    }
            );
        */
    }

    void ReadItemsFromFile(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string currentTitle = "";
        List<string> currentLines = new List<string>();
        foreach (string line in lines)
        {
            // Check if the line starts with "**" indicating a title:
            if (line.StartsWith("**") && line.EndsWith("**"))
            {
                // If a title is already being processed, add it to the ItemsList:
                if (!string.IsNullOrEmpty(currentTitle))
                {
                    ItemsList.Add((currentTitle, currentLines));
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
        // Add the last set of Items (if any) to the ItemsList:
        if (!string.IsNullOrEmpty(currentTitle))
        {
            ItemsList.Add((currentTitle, currentLines));
        }
    }
    
    void AddItemsToFile(string title, List<string> lines)
    {
        string existingContent = File.ReadAllText(filePath);
        // Add title with format **title**
        string newItems = $"\n**{title}**";
        foreach (string line in lines)
        {
            newItems += $"\n{line}";
        }
        // Append new lines
        string updatedContent = existingContent + newItems;
        // Add to variable
        ItemsList.Add((title, lines));
        // Write to file
        File.WriteAllText(filePath, updatedContent);
    }

    public List<(string, List<string>)> getItemsList(){return ItemsList;}
}
