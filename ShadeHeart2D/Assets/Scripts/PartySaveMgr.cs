using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class PartySaveMgr
{
    public static void SaveParty(Shade[] party)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string saveDataPath = Application.persistentDataPath + "/party.sav";
        Debug.Log(saveDataPath);
        FileStream fileStream = new FileStream(saveDataPath, FileMode.Create);
        //PartyData partyData = new PartyData(party);

        //binaryFormatter.Serialize(fileStream, partyData);
        fileStream.Close();
    }

    public static PartyData LoadParty()
    {
        string saveDataPath = Application.persistentDataPath + "/party.sav";
        if (File.Exists(saveDataPath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(saveDataPath, FileMode.Open);
            PartyData partyData = binaryFormatter.Deserialize(fileStream) as PartyData;
            fileStream.Close();
            return partyData;
        }
        else
        {
            Debug.LogError($"Party data not found in {saveDataPath}");
            return null;
        }
    }
}
