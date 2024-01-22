using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Effect
{
    public enum EffectType
    {
        Damage,
        Heal,
        Stun,
        Buff,
        Debuff,
        Defense
    }

    public int _duration;
    public EffectType _type;
    public int _amount;
}
