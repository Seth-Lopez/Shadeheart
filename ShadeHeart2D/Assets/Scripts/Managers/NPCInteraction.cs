using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    private UIMenuMngr UIClass;
    private bool isPlayerInRange = false;
    private bool isTextBoxSet = false;
    private string currentNPC = "";
    private void Start() 
    {
        UIClass = GameObject.FindGameObjectWithTag("UIMngr").GetComponent<UIMenuMngr>();
        try 
        {
            dialogueText = UIClass.getDialogueText();
            isTextBoxSet = true;
        }
        catch{ isTextBoxSet = false; }
        
    }
    private void Update()
    {
        if(!isTextBoxSet)
        {
            try 
            {
                dialogueText = UIClass.getDialogueText();
                isTextBoxSet = true;
            }
            catch{ isTextBoxSet = false; Debug.Log("Warning Text Box Not Set Yet For: " + this.gameObject.name);}
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            currentNPC = this.transform.name;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueText.text = "";
            currentNPC = "";
        }
        UIClass.openDialogueBox = false;
    }

    public void ShowDialogue(List<(string, bool)> dialogue)
    {
        foreach ((string line, bool hasOptions) in dialogue)
        {
            if(hasOptions)
                UIClass.setHasOptions(true);
            else
                UIClass.setHasOptions(false);
            if(UIClass.openDialogueBox == false)
            {   
                UIClass.openDialogueBox = true;
            }
            dialogueText.text = line;
        }
    }
    public bool getIsPlayerInRange()
    {
        return isPlayerInRange;
    }
    public string getCurrentNPC()
    {
        return currentNPC;
    }
    public void emptyDialogueText()
    {
        dialogueText.text = "";
    }
}

