using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyData
{
    static GameObject temp;
    public static int partySize;
    public static GameObject[] party = { temp, temp, temp, temp, temp, temp, temp, temp, temp };
    public static bool saved = false;

    /*
    public string[] name;
    public int[] level;

    public float[] baseMaxHealth;
    public float[] baseMaxEnergy;
    public int[] baseAttack;
    public int[] baseDefense;
    public int[] baseSpeed;

    public int[] spriteIndex;

    public int[] exp;
    public int[] lightLevels;
    public int[] darkLevels;
    public int[] levelExp;
    public int[] totalExp;
    public int[] requiredEXP;
    public int[] baseRequiredExp;

    public int[] baseExpYield;
    //
    public int[] weakness;

    public string[][] skillNames;
    public float[][] skillPowers;
    public int[][] skillCosts;
    public int[][] skillDamageTypes;

    public bool[][] skillIsTargetSelf;
    public bool[][] skillEffectTargetSelf;
    public int[][] skillEffects;
    public int[][] skillAnimationTypes;

    public string[][] skillDescriptions;
    
    public PartyData(Shade[] party)
    {
        for (int i = 0; i < party.Length; i++)
        {
            Debug.Log(party[i].name);
            name[i] = party[i].name;
            level[i] = party[i].level;

            baseMaxHealth[i] = party[i].baseMaxHealth;
            baseMaxEnergy[i] = party[i].baseMaxEnergy;
            baseAttack[i] = party[i].baseAttack;
            baseDefense[i] = party[i].baseDefense;
            baseSpeed[i] = party[i].baseSpeed;
            spriteIndex[i] = party[i].index;
            exp[i] = party[i].exp;
            lightLevels[i] = party[i].lightLevels;
            darkLevels[i] = party[i].darkLevels;
            levelExp[i] = party[i].levelExp;
            totalExp[i] = party[i].totalExp;
            requiredEXP[i] = party[i].requiredEXP;
            baseRequiredExp[i] = party[i].baseRequiredExp;
            weakness[i] = ((int)party[i].weakness);
            for (int j = 0; j < party[i].activeSkills.Count; j++)
            {
                skillNames[i][j] = party[i].activeSkills[j].name;
                skillPowers[i][j] = party[i].activeSkills[j].power;
                skillCosts[i][j] = party[i].activeSkills[j].cost;
                skillDamageTypes[i][j] = ((int)party[i].activeSkills[j].damageType);
                skillIsTargetSelf[i][j] = party[i].activeSkills[j].isTargetSelf;
                skillEffectTargetSelf[i][j] = party[i].activeSkills[j].effectTargetSelf;
                skillEffects[i][j] = ((int)party[i].activeSkills[j].effect);
                skillAnimationTypes[i][j] = ((int)party[i].activeSkills[j].animationType);
                skillDescriptions[i][j] = party[i].activeSkills[j].description;
            }
        }
    }*/
}