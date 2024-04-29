using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    private List<string> dialogueSpoken = new List<string>();
    private List<(string, List<string>, List<(string, bool, bool)>)> dialogueList = new List<(string, List<string>, List<(string, bool, bool)>)>();
    private List<(string, List<string>, List<(string, bool, bool)>)> questDialogueList = new List<(string, List<string>, List<(string, bool, bool)>)>();
    public List<string> allDialogueOptions = new List<string>(); 
    private List<(string, bool)> allQuestDialogueOptions = new List<(string, bool)>(); 
    private List<string> sortedDialogueOptions = new List<string>();
    private int type = -1;
    private NPCInteraction npcInter;
    private UIMenuMngr UIClass;
    private QuestMngrV2 queMngr;
    private bool isPauseMenuOpen = false;
    public int numLines = 0;
    private int runOnce = 0;
    private bool hasQuest = false;
    private List<(string, bool)> nextQuestLine = new List<(string, bool)>();
    public List<string> getAllDialogueOptions
    {
        get { return allDialogueOptions; }
    }

    private void Start() 
    {
        UIClass = GameObject.FindGameObjectWithTag("UIMngr").GetComponent<UIMenuMngr>();
        npcInter = GetComponent<NPCInteraction>();
        queMngr = GameObject.FindGameObjectWithTag("QuestMngrV2").GetComponent<QuestMngrV2>();
        instantiateVariables();
        if(type != 0)
        {
            retreiveDialogueOptions(false, 0);
        }
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            isPauseMenuOpen = !isPauseMenuOpen;
        if(UIClass.getIsMenuOpen())
        {
            npcInter.emptyDialogueText();
            UIClass.openDialogueBox = false;
            UIClass.setCurrentNPC(null);
            UIClass.isTalking(false);
            runOnce = 0;
        }
        else
            if (npcInter.getIsPlayerInRange() && Input.GetKeyDown(KeyCode.E) && !UIClass.getIsPauseMenuOpen())
            {
                if(UIClass != null)
                {
                    UIClass.openDialogueBox = true;
                    if(allQuestDialogueOptions != null)
                    {
                        foreach((string lines, bool hasOptions) in allQuestDialogueOptions)
                        {
                            if (lines == "" || lines == null)
                            {
                                hasQuest = false;
                            }
                            else
                            {
                                hasQuest = true;
                            }
                        }
                    } else {hasQuest = false;}
                    if(!hasQuest)
                    {
                        List<(string, bool)> parameter = new List<(string, bool)> { (new List<string> { nextDialogue(allDialogueOptions) }[0], false) };
                        npcInter.ShowDialogue(parameter);
                    }
                    else
                    {
                        bool isQuestCompleted = false;
                        foreach(QuestMngrV2.Quest quest in queMngr.getQuests())
                        {
                            if(quest.npcID == this.gameObject.name)
                            {
                                    isQuestCompleted = quest.isCompleted;
                            }
                        }
                        if(!isQuestCompleted)
                        {
                            (string text, bool option) = allQuestDialogueOptions[0];
                            List<(string, bool)> parameter = new List<(string, bool)> {(text, option)};
                            try
                            {
                                (text, option) = allQuestDialogueOptions[1];
                                nextQuestLine = new List<(string, bool)> {(text, option)};
                            }
                            catch{}
                            npcInter.ShowDialogue(parameter);
                        }
                        else if(isQuestCompleted)
                        {
                            GameObject.FindAnyObjectByType<DialogueMngr>().writeToQuestFile(this.gameObject, 1); //HERE IS A TERRIBLE THING!
                            retreiveDialogueOptions(true, 1);
                            npcInter.ShowDialogue(allQuestDialogueOptions);
                        }
                    }
                    if(runOnce == 0)
                    {
                        UIClass.setCurrentNPC(this.gameObject);
                        UIClass.isTalking(true);
                    }
                }
                else
                    Debug.Log("Dialogue Box Missing");
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
    public void retreiveDialogueOptions(bool removeLines, int repeat)
    {
        DialogueMngr dMngr = GameObject.Find("DialogueMngr").GetComponent<DialogueMngr>();
        dialogueList = dMngr.getDialogueList();
        questDialogueList = dMngr.getQuestDialogueList();
        if(dialogueList == null)
        {
            Debug.Log("Dialogue script not found... :/");
        }
        if(questDialogueList == null)
        {
            Debug.Log("QuestDialogue script not found... :/");
        }
        setAllDialogueOptions();
        for( int i = 0; i < repeat; i++)
        {
            if(removeLines)
            {
                allQuestDialogueOptions.RemoveAt(0);
            }
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
        allDialogueOptions = new List<string>(); 
        allQuestDialogueOptions = new List<(string, bool)>(); 
        foreach ((string title, List<string> lines, List<(string, bool, bool)> trash) in dialogueList)
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
        foreach ((string title, List<string> trash, List<(string, bool, bool)> lines) in questDialogueList)
        {
            if(title == this.transform.name)
            {
                foreach((string line, bool options, bool hasBeenRead) in lines)
                {
                    numLines++;
                    if(!hasBeenRead)
                    {
                        allQuestDialogueOptions.Add((line, options));
                    }
                }
            }
        }
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
            int randomIndex = UnityEngine.Random.Range(0, sortedDialogueOptions.Count);
            addSpokenDialogue(sortedDialogueOptions[randomIndex]);
            return sortedDialogueOptions[randomIndex];
        }
        else
        {
            return "I already told you all I know.";
        }
    }
    public void resetRunOnce(){ runOnce = 0;}
    public List<(string, bool)> getNextLine(){return nextQuestLine;}
}
