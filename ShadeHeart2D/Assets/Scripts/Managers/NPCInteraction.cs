using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    private bool isPlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("True 1");
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            //Debug.Log("True 2");
            isPlayerInRange = false;
            dialogueText.text = "";
        }
    }

    public void ShowDialogue(string npcDialogue)
    {
        dialogueText.text = npcDialogue;
    }
    public bool getIsPlayerInRange()
    {
        return isPlayerInRange;
    }
}

