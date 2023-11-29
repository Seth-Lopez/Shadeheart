using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shade : MonoBehaviour
{

    public Meter hpBar, energyBar;
    public float health, maxHealth, energy, maxEnergy;
    public string name;
    public bool charged = false;
    public bool defending = false;

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
        hpBar.SetValue(health, maxHealth);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (health < 0)
        {
            health = 0;
        }
    }

    public void UpdateEnergy(float energyUsed)
    {
        energy -= energyUsed;
        energyBar.SetValue(energy, maxEnergy);
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
        if (energy < 0)
        {
            energy = 0;
        }
    }
    /*
    public void keepUnderMaxHealth()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void keepUnderMaxEnergy()
    {
        if (energy > maxEnergy)
        {
            energy = maxEnergy;
        }
    }
    */
}