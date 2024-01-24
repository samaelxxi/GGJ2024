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

    public Effect(EffectType type, int amount, int duration)
    {
        Type = type;
        Amount = amount;
        Duration = duration;
    }
}
