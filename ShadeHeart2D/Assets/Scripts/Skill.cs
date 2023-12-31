using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string name;
    public float power;
    public int cost;
    public DamageType damageType;
    public Shade target, user, effectTarget;
    public bool isTargetSelf, effectTargetSelf;
    public Effect effect;
}
