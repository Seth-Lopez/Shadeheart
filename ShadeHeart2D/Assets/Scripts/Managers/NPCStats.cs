using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPCStats : MonoBehaviour
{
    private GameState gameState;
    private List<string> dialogueSpoken = new List<string>();
    private List<(string, List<string>)> dialogueList = new List<(string, List<string>)>();
    private NPCMngr nPC;
    private List<string> allDialogueOptions = new List<string>(); 
    private List<string> sortedDialogueOptions = new List<string>();
    private int type = -1;
    private NPCInteraction npcInter; 
    private void Start() 
    {
        npcInter = GetComponent<NPCInteraction>();
        instantiateVariables();
        if(type != 0)
        {
            retreiveDialogueOptions();
            setAllDialogueOptions();
        }
    }
    
    void Update()
    {
        if (npcInter.getIsPlayerInRange() && Input.GetKeyDown(KeyCode.E))
        {
            npcInter.ShowDialogue(nextDialogue());
        }
    }
    private void instantiateVariables()
    {
        gameState = GameObject.FindObjectOfType<GameState>();
        List<(int, bool)> GameStateVar = gameState.getGameStateVar();
        foreach ((int intValue, bool boolValue) in GameStateVar)
        {
            Debug.Log($"Int Value: {intValue}, Bool Value: {boolValue}");
        }
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
        else
        {
            foreach ((string title, List<string> lines) in dialogueList)
            {
                Debug.Log($"Title: {title}");

                foreach (string line in lines)
                {
                    Debug.Log($"  Line: {line}");
                }

                Debug.Log("--------");
            }
        }
    }
    private void addSpokenDialogue(string line)
    {
        dialogueSpoken.Add(line);
    }
    private void setAllDialogueOptions()
    {
        foreach ((string title, List<string> lines) in dialogueList)
        {
            if(title == this.transform.name)
            {
                foreach(string line in lines)
                {
                    allDialogueOptions.Add(line);
                }
            }
            if(title == "generic")
            {
                foreach(string line in lines)
                {
                    allDialogueOptions.Add(line);
                }
            }
        }
    }
    private void setSortedDialogueOptions()
    {
        sortedDialogueOptions = allDialogueOptions;
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
    private string nextDialogue()
    {
        setSortedDialogueOptions();
        if (sortedDialogueOptions.Count > 0)
        {
            int randomIndex = Random.Range(0, sortedDialogueOptions.Count);
            addSpokenDialogue(sortedDialogueOptions[randomIndex]);
            return sortedDialogueOptions[randomIndex];
        }
        else
        {
            return "I got to go it was nice chatting with you.";
        }
    }
}
