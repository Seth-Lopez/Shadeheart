using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyData
{
    public ShadeData[] partyData;
    public PartyData(Shade[] party)
    {
        for (int i = 0; i < party.Length; i++)
        {
            partyData[i] = new ShadeData(party[i]);
        }
    }
}

[System.Serializable]
public class ShadeData
{
    public string name;
    public int level;

    public float baseMaxHealth, baseMaxEnergy;
    public int baseAttack, baseDefense, baseSpeed;

    public int spriteIndex;

    public int exp;
    public int lightLevels = 0;
    public int darkLevels = 0;
    public int levelExp;
    public int totalExp;
    public int requiredEXP = 300;
    public int baseRequiredExp = 300;

    public int baseExpYield;

    //
    public string[] skillNames;
    public float[] skillPowers;
    public int[] skillCosts;
    public int[] skillDamageTypes;

    public bool[] skillIsTargetSelf;
    public bool[] skillEffectTargetSelf;
    public int[] skillEffects;
    public int[] skillAnimationTypes;
    
    public string[] skillDescriptions;

    public ShadeData(Shade shade)
    {
        name = shade.name;
        level = shade.level;

        baseMaxHealth = shade.baseMaxHealth;
        baseMaxEnergy = shade.baseMaxEnergy;
        baseAttack = shade.baseAttack;
        baseDefense = shade.baseDefense;
        baseSpeed = shade.baseSpeed;
        //sprite index
        exp = shade.exp;
        lightLevels = shade.lightLevels;
        darkLevels = shade.darkLevels;
        levelExp = shade.levelExp;
        totalExp = shade.totalExp;
        requiredEXP = shade.requiredEXP;
        baseRequiredExp = shade.baseRequiredExp;
        for (int i = 0; i < shade.activeSkills.Count; i++)
        {
            skillNames[i] = shade.activeSkills[i].name;
            skillPowers[i] = shade.activeSkills[i].power;
            skillCosts[i] = shade.activeSkills[i].cost;
            skillDamageTypes[i] = ((int)shade.activeSkills[i].damageType);
            skillIsTargetSelf[i] = shade.activeSkills[i].isTargetSelf;
            skillEffectTargetSelf[i] = shade.activeSkills[i].effectTargetSelf;
            skillEffects[i] = ((int)shade.activeSkills[i].effect);
            skillAnimationTypes[i] = ((int)shade.activeSkills[i].animationType);
            skillDescriptions[i] = shade.activeSkills[i].description;
        }

    }
}
