using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class resetGame : MonoBehaviour
{
    private string filePath = Path.Combine(Application.streamingAssetsPath, "Managers/DialogueOptions.txt");
    private string filePath2 = Path.Combine(Application.streamingAssetsPath, "Managers/QuestDialogueOptions.txt");
    
    private string filePath3 = Path.Combine(Application.streamingAssetsPath, "Managers/DialogueOptionsCopy.txt");
    private string filePath4 = Path.Combine(Application.streamingAssetsPath, "Managers/QuestDialogueOptionsCopy.txt");

    private string filePath5 = Path.Combine(Application.streamingAssetsPath, "Managers/InventoryItems.txt");
    private string filePath6 = Path.Combine(Application.streamingAssetsPath, "Managers/InventoryItemsCopy.txt");

    public void resetGameButton()
    {
        string sourceFilePath = filePath3;
        string destinationFilePath = filePath;
        string fileContents = File.ReadAllText(sourceFilePath);
        File.WriteAllText(destinationFilePath, fileContents);
        sourceFilePath = filePath4;
        destinationFilePath = filePath2;
        fileContents = File.ReadAllText(sourceFilePath);
        File.WriteAllText(destinationFilePath, fileContents);
        sourceFilePath = filePath6;
        destinationFilePath = filePath5;
        fileContents = File.ReadAllText(sourceFilePath);
        File.WriteAllText(destinationFilePath, fileContents);
    }
}
