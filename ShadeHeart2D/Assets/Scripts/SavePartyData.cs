using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePartyData : MonoBehaviour
{
    public void SaveParty(GameObject[] shades)
    {
        for (int i = 0; i < shades.Length; i++)
        {
            PartyData.party[i] = shades[i];
        }
    }
}
