using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum DamageType { None, Strike, Slash, Fire, Electric, Ice, Light, Dark , Heal }
public enum Effect { None, Defend, Charge, Shock, Burn, Blind, Freeze }

public class Shade : MonoBehaviour
{
    public string name;
    public Meter hpBar, energyBar;
    public float health, maxHealth, energy, maxEnergy;
    public float attack, defense;
    public int speed;
    public DamageType basicAttackType, weakness;
    public bool isCharged = false;
    public bool isDefending = false;
    public Skill[] activeSkills;

    public void SetupHealthBar()
    {
        hpBar.SetMaxValue(maxHealth);
    }

    public void SetupEnergyBar()
    {
        energyBar.SetMaxValue(maxEnergy);
    }

    public void UpdateHealth(float damage)
    {
        health -= damage;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health < 0)
        {
            health = 0;
        }
        hpBar.SetValue(health);
    }

    public void UpdateEnergy(float energyUsed)
    {
        energy -= energyUsed;
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
        if (energy < 0)
        {
            energy = 0;
        }
        energyBar.SetValue(energy);
    }
}