using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum DamageType { None, Strike, Slash, Fire, Electric, Ice, Light, Dark , Heal }
public enum Effect { None, Defend, Charge, Stun, Burn, Blind, Freeze/*, Shock*/ }

[System.Serializable]
public class Shade : MonoBehaviour
{
    new public string name;
    public Meter hpBar, energyBar;
    public int level;
    public Sprite sprite;

    [SerializeField] float maxHealth, maxEnergy;
    public float MaxHealth
    {
        get { return Mathf.Round(((baseMaxHealth / 25) * level) + 20); }
        set { maxHealth = value; }
    }

    public float MaxEnergy
    {
        get { return Mathf.Round(((baseMaxEnergy / 25) * level) + 5); }
        set { maxEnergy = value; }
    }

    public float health, energy;

    [SerializeField] float attack, defense;

    public float Attack
    {
        get { return Mathf.Round(((baseAttack / 25) * level) + 10); }
    }

    public float Defense
    {
        get { return Mathf.Round(((baseDefense / 25f) * level) + 10); }
    }

    [SerializeField] int speed;
    public int Speed
    {
        get { return Mathf.RoundToInt(((baseSpeed / 25f) * level) + 10); }
    }

    //base stat variables
    public float baseMaxHealth, baseMaxEnergy;
    public int baseAttack, baseDefense, baseSpeed;

    public DamageType basicAttackType, weakness;

    //secondary effect variables
    public bool isCharged = false;
    public bool isDefending = false;
    public bool isStunned = false, wasStunned = false;
    public bool isShocked = false;
    public bool isBurned = false;
    public bool isFrozen = false;
    public int freezeIndex;

    //exp variables
    public int exp;
    public int lightLevels = 0;
    public int darkLevels = 0;
    public int levelExp;
    public int totalExp;
    public int requiredEXP = 300;
    public int baseRequiredExp = 300;

    public int baseExpYield;

    //skills
    public List<Skill> activeSkills;
    public List<LightSkill> lightSkills;
    public List<DarkSkill> darkSkills;
    //public List<Skill> knownSkills;


    public void SetupHealthBar()
    {
        hpBar.SetMaxValue(MaxHealth);
        hpBar.SetValue(health);
    }

    public void SetupEnergyBar()
    {
        energyBar.SetMaxValue(MaxEnergy);
        energyBar.SetValue(energy);
    }

    public void UpdateHealth(float damage)
    {
        health -= damage;
        if (health > MaxHealth)
        {
            health = MaxHealth;
        }
        if (health < 0)
        {
            health = 0;
        }
        hpBar.ChangeValue(health);
    }

    public void UpdateEnergy(float energyUsed)
    {
        energy -= energyUsed;
        if (energy > MaxEnergy)
        {
            energy = MaxEnergy;
        }
        if (energy < 0)
        {
            energy = 0;
        }
        energyBar.ChangeValue(energy);
    }

    public void SetupInitialEXP()
    {
        totalExp = 0;
        for (int i = level; i > 1; i--)
        {
            totalExp += (requiredEXP * level-1);
        }
        exp = 0;
        lightLevels = 0;
        darkLevels = level-1;
        levelExp = 0;
    }

    public int expCalc(int expYield, int enemyLevel)
    {
        return ((expYield * enemyLevel) + Mathf.RoundToInt(((enemyLevel - level) * expYield) * 0.2f));
    }

    public bool UpdateEXP(int expGain)
    {
        exp += expGain;

        totalExp += Mathf.Abs(expGain);
        levelExp += Mathf.Abs(expGain);

        bool leveled = false;
        while (levelExp >= (requiredEXP * level))
        {
            levelExp -= (requiredEXP * level);
            LevelUp();
            leveled = true;
        }

        if (leveled)
        {
            if (levelExp == 0)
            {
                exp = 0;
            }
            else if (levelExp > 0)
            {
                if (exp < 0)
                {
                    exp = -levelExp;
                }
                else
                {
                    exp = levelExp;
                }
            }
        }
        return leveled;
    }

    public void LevelUp()
    {
        level++;
        if (exp >= 0)
        {
            lightLevels++;
        }
        else
        {
            darkLevels++;
        }
        //LearnSkill();
    }

    public void LearnSkill()
    {
        if (exp >= 0)
        {
            foreach (var skill in lightSkills)
            {
                if (skill.Level <= lightLevels && skill.learned == false)
                {
                    skill.learned = true;

                    if (activeSkills.Count >= 4)
                    {
                        //ask player which skill to remove
                        activeSkills.RemoveAt(0);
                    }
                    activeSkills.Add(skill.BaseSkill);
                }
            }
        }
        else
        {
            foreach (var skill in darkSkills)
            {
                if (skill.Level <= darkLevels && skill.learned == false)
                {
                    skill.learned = true;

                    if (activeSkills.Count >= 4)
                    {
                        //ask player which skill to remove
                        activeSkills.RemoveAt(0);
                    }
                    activeSkills.Add(skill.BaseSkill);
                }
            }
        }
    }
    /*
    public Skill GetPotentialSkill()
    {
        if (exp >= 0)
        {
            foreach (var skill in lightSkills)
            {
                if (skill.Level <= lightLevels && skill.learned == false)
                {
                    skill.learned = true;

                    return skill.BaseSkill;
                }
            }
        }
        else
        {
            foreach (var skill in darkSkills)
            {
                if (skill.Level <= darkLevels && skill.learned == false)
                {
                    skill.learned = true;

                    return skill.BaseSkill;
                }
            }
        }

        return new Skill();
    }

    public void LearnSkill(Skill skill, int index)
    {
        if (activeSkills.Count >= 4)
        {
            if (index <= 4)
            {
                activeSkills.RemoveAt(index);
            }
        }
        activeSkills.Add(skill);
    }
    */
    public void SetupSkills()
    {
        activeSkills = new List<Skill>();
        foreach (var skill in darkSkills)
        {
            if (skill.Level <= level)
            {
                if (activeSkills.Count >= 4)
                {
                    activeSkills.RemoveAt(0);
                }
                activeSkills.Add(skill.BaseSkill);
                skill.learned = true;
            }
        }
    }

    public void SetSkills()
    {

    }
}

[System.Serializable]
public class LightSkill
{
    [SerializeField] Skill baseSkill;
    [SerializeField] int level;
    public bool learned = false;
    [SerializeField] int skillPoints;

    public Skill BaseSkill
    {
        get { return baseSkill; }
    }

    public int Level
    {
        get { return level; }
    }

    public int SkillPoints
    {
        get { return skillPoints; }
    }
}
[System.Serializable]
public class DarkSkill
{
    [SerializeField] Skill baseSkill;
    [SerializeField] int level;
    public bool learned = false;
    [SerializeField] int skillPoints;

    public Skill BaseSkill
    {
        get { return baseSkill; }
    }

    public int Level
    {
        get { return level; }
    }

    public int SkillPoints
    {
        get { return skillPoints; }
    }
}