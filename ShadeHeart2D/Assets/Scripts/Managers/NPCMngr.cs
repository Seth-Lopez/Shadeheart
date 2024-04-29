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
{
    [SerializeField] private GameObject npcPrefab;
    private List<string> npcNames = new List<string>();
    private string filePath = Path.Combine(Application.streamingAssetsPath, "Managers/NPCNames.txt");
    void Start()
    {
        setnpcNames();
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