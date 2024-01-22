using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Damage, Heal, Stun, Buff, Debuff, Defense
}


public class Effect
{


    public int Duration;
    public EffectType Type;
    public int Amount;
}
