using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPartyData : MonoBehaviour
{
    public void LoadParty(GameObject[] playerParty)
    {
        for (int i = 0; i < PartyData.party.Length; i++)
        {
            playerParty[i] = PartyData.party[i];
        }
    }
}
