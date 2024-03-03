using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum DamageType { None, Strike, Slash, Fire, Electric, Ice, Light, Dark , Heal }
public enum Effect { None, Defend, Charge, Stun, Burn, Blind, Freeze }

public class Shade : MonoBehaviour
{
    public string name;
    public Meter hpBar, energyBar;
    public int level;

    [SerializeField] float maxHealth, maxEnergy;
    public float MaxHealth
    {
        get { return Mathf.Round(((maxHealth / 25) * level) + 20); }
    }
    public float MaxEnergy
    {
        get { return Mathf.Round(((maxEnergy / 25) * level) + 5); }
    }
    public float health, energy;

    [SerializeField] float attack, defense;
    public float Attack
    {
        get { return Mathf.Round(((attack / 25) * level) + 10); }
    }
    public float Defense
    {
        get { return Mathf.Round(((defense / 25f) * level) + 10); }
    }

    [SerializeField] int speed;
    public int Speed
    {
        get { return Mathf.RoundToInt(((speed / 25f) * level) + 10); }
    }
    public DamageType basicAttackType, weakness;
    public bool isCharged = false;
    public bool isDefending = false;
    public bool isStunned = false, wasStunned = false;
    public Skill[] activeSkills;

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
}