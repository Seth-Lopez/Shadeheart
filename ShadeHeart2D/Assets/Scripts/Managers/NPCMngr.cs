using System.Collections.Generic;
using UnityEngine;
using System.IO;
public enum NPCType
{
    monster,
    questGiver,
    normal
}

public class NPCMngr: MonoBehaviour
{/*
    protected string npcName;
    protected NPCType occupation;
    protected List<string> dialogueOptions;*/
    [SerializeField] private GameObject npcPrefab;
    private List<string> npcNames = new List<string>();
    private string filePath = Path.Combine(Application.dataPath, "Scripts/Managers/NPCNames.txt");
    // private string filePath = Path.Combine(Application.streamingAssetsPath, "Scripts/Managers/GameState.txt");
    
    /*public NPCMngr(string npcName, int occupation, List<string> dialogueOptions)
    {
        this.npcName = npcName;
        this.dialogueOptions = dialogueOptions;
        if(occupation == 0)
            this.occupation = NPCType.monster;
        else if(occupation == 1)
            this.occupation = NPCType.questGiver;
        else if(occupation == 0)
            this.occupation = NPCType.normal;
    }*/
    void Start()
    {
        setnpcNames();
        //GameObject npc;
        //Transform parent;
        /*
        for(int i =0; i < npcNames.Count; i++)
        {
            npc = Instantiate(npcPrefab, new Vector3(-23.62f, -1.352248f, -5.569558f), Quaternion.identity);
            parent = GameObject.Find("NPC - Quest Givers").transform;
            npc.transform.SetParent(parent);
            npc.transform.name = npcNames[i];
        }*/
    }
    private void setnpcNames()
    {
        string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] names = line.Split(',');
                foreach (string name in names)
                {
                    npcNames.Add(name);
                }
            }
    }
    
}