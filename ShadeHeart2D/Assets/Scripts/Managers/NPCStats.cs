using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using TMPro;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    private List<string> dialogueSpoken = new List<string>();
    private List<(string, List<string>, List<(int, string, bool, string)>, List<(int, string, bool, string)>)> dialogueList = new List<(string, List<string>, List<(int, string, bool, string)>, List<(int, string, bool, string)>)>();
    private NPCMngr nPC;
    private List<string> allDialogueOptions = new List<string>(); 
    private List<(string, bool)> allQuestDialogueOptions = new List<(string, bool)>();
    private List<(string, bool)> spokenQuestDialogueOptions = new List<(string, bool)>();
    private List<(string, bool)> resetQuestDialogueOptions = new List<(string, bool)>(); 
    private List<string> sortedDialogueOptions = new List<string>();
    private int type = -1;
    private NPCInteraction npcInter;
    private UIMenuMngr UIClass;
    private bool isPauseMenuOpen = false;
    public int numLines = 0;
    public List<string> getAllDialogueOptions
    {
        get { return allDialogueOptions; }
    }
    public List<(string, bool)> getAllQuestDialogueOptions
    {
        get { return allQuestDialogueOptions; }
    }
    private bool hasQuest = false;
    private bool hasSelectedNewQuest = false;
    private int numOfLinesSaid = 0;

    private void Start() 
    {
        UIClass = GameObject.FindGameObjectWithTag("UIMngr").GetComponent<UIMenuMngr>();
        npcInter = GetComponent<NPCInteraction>();
        instantiateVariables();
        if(type != 0)
        {
            retreiveDialogueOptions();
            setAllDialogueOptions();
            
        }
        checkedHasQuests();
    }
    
    void Update()
    {
        if(UIClass.getShouldResetQuest())
        {
            UIClass.setShouldResetQuest(false);
            allQuestDialogueOptions = resetQuestDialogueOptions;
            numOfLinesSaid = 0;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
            isPauseMenuOpen = !isPauseMenuOpen;
        if(UIClass.getIsMenuOpen())
        {
            npcInter.emptyDialogueText();
            UIClass.openDialogueBox = false;
            UIClass.setCurrentNPC("");
            hasSelectedNewQuest = false;
        }
        else
            if (npcInter.getIsPlayerInRange() && Input.GetKeyDown(KeyCode.E) && !UIClass.getIsPauseMenuOpen())
            {
                if(UIClass != null)
                {
                    UIClass.openDialogueBox = true;
                    UIClass.setCurrentNPC(npcInter.getCurrentNPC());
                    hasSelectedNewQuest = UIClass.getHasSelectedNewQuest();
                    if(hasSelectedNewQuest)
                        showNextText();
                }
                else
                    Debug.Log("Dialogue Box Missing");
                if(hasQuest)
                {
                    npcInter.ShowDialogue(nextQuestDialogue(allQuestDialogueOptions));
                }
                else
                {
                    List<(string, bool)> parameter = new List<(string, bool)> { (new List<string> { nextDialogue(allDialogueOptions) }[0], false) };
                    npcInter.ShowDialogue(parameter);
                }
            }
            else if(npcInter.getIsPlayerInRange() == false && UIClass.getHasSelectedNewQuest() == false && hasQuest)
            {
                Debug.Log("HEEEEE");
                numOfLinesSaid -= 1;
            }
    }
    private List<(string, bool)> nextQuestDialogue(List<(string, bool)> dialogue)
    {
        string [] arrLines = new string[dialogue.Count]; 
        bool [] arrBools = new bool[dialogue.Count]; 
        if(allDialogueOptions.Count != 0)
        {
            int count = 0;
            foreach ((string line, bool hasOptions) in dialogue)
            {
                arrLines[count] = line;
                arrBools[count] = hasOptions;
                count++;
            }

            bool skip = false;
            count = 0;
            hasSelectedNewQuest = UIClass.getHasSelectedNewQuest();
            if(hasSelectedNewQuest)
                skip = true;
            if(numOfLinesSaid >= dialogue.Count)
            {
                numOfLinesSaid = dialogue.Count-1;
            }
            if(numOfLinesSaid < 0)
                numOfLinesSaid = 0;
            if(hasSelectedNewQuest || arrBools[numOfLinesSaid])
            {
                hasSelectedNewQuest = false;
                UIClass.setHasSelectedNewQuest(false);
            }       
            numOfLinesSaid+=1;
            if(!skip)
            {
                return new List<(string, bool)> { (arrLines[numOfLinesSaid-1], arrBools[numOfLinesSaid-1]) };
            }

        }
        else
        {
            hasQuest = false;
            return new List<(string, bool)> { ("", false) };
        }
        try
        {
            numOfLinesSaid += 1;
            return new List<(string, bool)> { (arrLines[numOfLinesSaid], arrBools[numOfLinesSaid]) };
        }
        catch
        {
            return new List<(string, bool)> { (new List<string> { nextDialogue(allDialogueOptions) }[0], false) };
        }
    }
    private void checkedHasQuests()
    {
        int count = 0;
        foreach ((string title, List<string> lines, List<(int, string, bool, string)> preQuestLines, List<(int, string, bool, string)> postQuestLines) in dialogueList)
        {
            if(title == this.transform.name)
            {
                foreach ((int number, string option, bool hasOptions, string text) line in preQuestLines)
                {
                    count++;
                    allQuestDialogueOptions.Add((line.text, line.hasOptions));
                }
                foreach ((int number, string option, bool hasOptions, string text) line in preQuestLines)
                {
                    count++;
                    allQuestDialogueOptions.Add((line.text, line.hasOptions));
                }
            }
        }
        if(count != 0)
        {
            hasQuest = true;
            resetQuestDialogueOptions = allQuestDialogueOptions;
        }
    }
    private void instantiateVariables()
    {
        string parentName = transform.parent.gameObject.name;   
        if(parentName == "NPC - Monsters")
            type = 0;
        else if(parentName == "NPC - Quest Givers")
            type = 1;
        else if(parentName == "NPC - Citizens")
            type = 2;
    }
    private void retreiveDialogueOptions()
    {
        DialogueMngr dMngr = GameObject.Find("DialogueMngr").GetComponent<DialogueMngr>();
        dialogueList = dMngr.getDialogueList();
        if(dialogueList == null)
        {
            Debug.Log("Dialogue script not found... :/");
        }
    }
    private void addSpokenDialogue(string line)
    {
        dialogueSpoken.Add(line);
    }
    private void setAllDialogueOptions()
    {
        //List<(string, List<string>, List<(int, string, bool, string)>, List<(int, string, bool, string)>)>
        numLines = 0; //reset numLines counter
        foreach ((string title, List<string> lines, List<(int, string, bool, string)> preQuestLines, List<(int, string, bool, string)> postQuestLines) in dialogueList)
        {
            if(title == this.transform.name)
            {
                foreach(string line in lines)
                {

                    numLines++;
                    allDialogueOptions.Add(line);
                }
            }
            if(title == "generic")
            {
                foreach(string line in lines)
                {
                    numLines++;
                    allDialogueOptions.Add(line);
                }
            }
        }
        // save counter 
    }
    private void setSortedDialogueOptions(List<string> dialogue)
    {
        sortedDialogueOptions = dialogue;
        for (int i = sortedDialogueOptions.Count - 1; i >= 0; i--)
        {
            foreach (string lineToRemove in dialogueSpoken)
            {
                if (sortedDialogueOptions[i] == lineToRemove)
                {
                    sortedDialogueOptions.RemoveAt(i);
                    break;
                }
            }
        }
    }
    private string nextDialogue(List<string> dialogue)
    {
        setSortedDialogueOptions(dialogue);
        if (sortedDialogueOptions.Count > 0)
        {
            int randomIndex = Random.Range(0, sortedDialogueOptions.Count);
            addSpokenDialogue(sortedDialogueOptions[randomIndex]);
            return sortedDialogueOptions[randomIndex];
        }
        else
        {
            return "I already told you all I know.";
        }
    }
    public void showNextText()
    {
        npcInter.ShowDialogue(nextQuestDialogue(allQuestDialogueOptions));
    }
}
