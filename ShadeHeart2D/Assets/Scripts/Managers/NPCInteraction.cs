using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    private UIMenuMngr UIClass;
    private bool isPlayerInRange = false;
    private void Start() 
    {
        UIClass = GameObject.FindGameObjectWithTag("UIMngr").GetComponent<UIMenuMngr>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueText.text = "";
        }
        UIClass.openDialogueBox = false;
    }

    public void ShowDialogue(string npcDialogue)
    {
        if(UIClass.openDialogueBox == false)
        {   
            UIClass.openDialogueBox = true;
            Debug.Log("Here");
        }
        Debug.Log("Here2");
        Debug.Log(npcDialogue + "ass");
        dialogueText.text = npcDialogue;
    }
    public bool getIsPlayerInRange()
    {
        return isPlayerInRange;
    }
    public void emptyDialogueText()
    {
        dialogueText.text = "";
    }
}

