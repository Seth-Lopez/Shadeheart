using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType { None, Slash, Electric, Electric2, Electric3, Icicle, Flame, Wave, Charge, Heal }

public class Skill : MonoBehaviour
{
    public string name;
    public float power;
    public int cost;
    public DamageType damageType;
    public Shade target, user, effectTarget;
    public bool isTargetSelf, effectTargetSelf;
    public Effect effect;
    public AnimationType animationType;
    [TextArea]
    public string description;
}
