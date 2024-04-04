using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using System.Linq;
using UnityEngine.UIElements;
using System.Threading;
using System.Linq.Expressions;

public class InventoryMngr : MonoBehaviour
{
    // The list that stores the Items data:
    private int numItems = 0;
    private List<(string, List<string>)> ItemsList = new List<(string, List<string>)>();
    private string filePath = Path.Combine(Application.dataPath, "Scripts/Managers/InventoryItems.txt");
    [SerializeField] private GameObject content;
    private TextMeshProUGUI inventoryList;

    public int NumItems
    {
        get { return numItems; }
    }

    void Awake()
    {
        inventoryList = content.GetComponent<TextMeshProUGUI>();
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
        //write to TextMeshPro
        updateInventory();
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
    //This function helps update the Inventory every time it wakes up to display correct items in inventory.
    private void updateInventory()
    {
        failChecker();
        inventoryList.text = "";
        foreach ((string title, List<string> lines) in ItemsList)
            foreach (string line in lines)
                inventoryList.text = inventoryList.text + line + "\n";
    }
     //This function is used for removing duplicate items in the inventory
    private void failChecker()
    {
        int combinednumber = 0;
        bool isFirst = true;
        List<(string, List<string>)> tempItemsList = new List<(string, List<string>)>(ItemsList);
        List<string> alreadyCombinedTitles = new List<string>();
        bool alreadyCombined = false;

        foreach ((string title1, List<string> lines1) in tempItemsList)
        {
            if(alreadyCombinedTitles.Count != 0)
            {
                foreach(string s in alreadyCombinedTitles)
                {
                    if(s == title1)
                        alreadyCombined = true;
                }
            }
            if(!alreadyCombined)
            {
                combinednumber += removeString(lines1[0]);
                foreach ((string title2, List<string> lines2) in tempItemsList)
                {
                    if (title1 == title2 && !ReferenceEquals(lines1, lines2) && !lines1.SequenceEqual(lines2))
                    {
                        if(combinednumber != 0 && isFirst == true)
                            isFirst = false;
                        combinednumber += removeString(lines2[0]);
                    }
                }
                alreadyCombinedTitles.Add(title1);
                if(combinednumber > 999)
                {
                    combinednumber = 999;
                }
                List<string> lines = new List<string>{"X" + combinednumber + "\t" + title1};
                if(isFirst)
                {
                    AddItemsToFile(title1, lines, true);
                }
                else
                {
                    AddItemsToFile(title1, lines, false);
                }
                if(combinednumber != 0 && isFirst == true)
                    isFirst = false;
                combinednumber = 0;
            }
            alreadyCombined = false;
        }
    }
     //This function is used for stripping strings to get the number of items in the line
    private int removeString(string str)
    {
        int number;
        if(str.Length > 3)
        {
            str = str.Substring(1, 3).TrimEnd().Split(' ')[0];
        }
        for (int i = str.Length; i >= 1;i--)
        {
            if(i >= 1 && int.TryParse(str.Substring(0,i), out number))
            {
                return number;
            }
        }
        Debug.Log("ERROR SHOULD NOT BE HERE!!!");
        return 0;
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
    //This function is used for interaction and will automatically add to inventory
    public void interactions(string Newtitle, List<string> Newlines)
    {
        int newInt = 0;
        foreach ((string title, List<string> lines) in ItemsList)
        {
            if(Newtitle == title)
            {
                newInt = removeString(Newlines[0]) + removeString(lines[0]);
                if(newInt > 999)
                {
                    newInt = 999;
                }
            }
        }
        if(newInt == 0)
            AddItemsToFile(Newtitle, Newlines, false);
        updateInventory();
    }
    //This function is used for adding items to file
    void AddItemsToFile(string title, List<string> lines, bool reWriting)
    {
        if(reWriting)
        {
            // Add title with format **title**
            string newItems = $"**{title}**";
            foreach (string line in lines)
            {
                newItems += $"\n{line}";
            }
            // Add to variable
            ItemsList = new List<(string, List<string>)>{(title, lines)};
            // Write to file
            File.WriteAllText(filePath, newItems);
        }
        else
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
    }

    public List<(string, List<string>)> getItemsList(){return ItemsList;}
}
