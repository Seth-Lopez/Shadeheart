using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questFixed : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindAnyObjectByType<QuestMngrV2>().setQuestsActiveComplete("oldlady", true, true);
        }
    }
}
