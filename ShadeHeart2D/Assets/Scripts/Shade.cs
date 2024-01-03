using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum DamageType { Strike, Slash, Fire, Electric, Ice, Light, Dark }

public class Shade : MonoBehaviour
{
    public string name;
    public Meter hpBar, energyBar;
    public float health, maxHealth, energy, maxEnergy;
    public float attack, defense;
    public int speed;
    public DamageType basicAttackType, weakness;
    public bool charged = false;
    public bool isDefending = false;

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
        hpBar.SetValue(health, maxHealth);
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
        energyBar.SetValue(energy, maxEnergy);
    }
}