using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questFixed : MonoBehaviour
{
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    /// 
    private QuestMngrV2 queMngr;
    void Update()
    {
        queMngr = GameObject.Find("QuestMngr").GetComponent<QuestMngrV2>();
        bool isActive = false;
        foreach(QuestMngrV2.Quest que in queMngr.getActiveQuests())
        {
            if(que.title == "Park Shade")
            {
                isActive = true;
            }
        }
        if(!isActive)
        {
            this.gameObject.SetActive(false);
        }
        foreach(QuestMngrV2.Quest que in queMngr.getCompletedQuests())
        {
            if(que.title == "Park Shade")
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindAnyObjectByType<QuestMngrV2>().setQuestsActiveComplete("oldlady", true, true);
        }
    }
}
