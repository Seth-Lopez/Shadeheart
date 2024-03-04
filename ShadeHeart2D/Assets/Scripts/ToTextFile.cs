using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System;
using UnityEditor;
public class ToTextFile : MonoBehaviour
{
    private String filePath = Path.Combine(Application.dataPath, "Scripts/Managers/");
    private List<String> nameOfFiles = new List<string>();
    private string[] txtFiles;
    void Start()
    {
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Managers/");
        createFile();
    }

    public void createFile()
    {
        getnameOfFile();
        int count = 0;
        foreach(String nameOfFile in nameOfFiles)
        {
            string txtDocumentName = Application.streamingAssetsPath + "/Managers/" + nameOfFile + ".txt";
            if(!File.Exists(txtDocumentName))
            {
                string[] lines = File.ReadAllLines(txtFiles[count]);
                File.WriteAllLines(txtDocumentName, lines);
            }
            count += 1;
        }
    }
    private void getnameOfFile()
    {
        
        txtFiles = Directory.GetFiles(filePath, "*.txt");
        foreach (string txtFile in txtFiles)
        {
            nameOfFiles.Add(Path.GetFileNameWithoutExtension(txtFile));
        }
    }
}
