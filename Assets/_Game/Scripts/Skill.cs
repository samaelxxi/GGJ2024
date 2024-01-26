using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


public enum SkillTarget { Self, Enemy, Ally, MyTeam }


[CreateAssetMenu(fileName = "Skill", menuName = "Skill", order = 1)]
public class Skill : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public SkillTarget Target { get; private set; }

    [field: SerializeField]
    public bool IsAttack { get; private set; }
    [field: SerializeField, ShowIf("IsAttack")]
    public int Damage { get; private set; }

    [field: SerializeField]
    public bool IsHeal { get; private set; }
    [field: SerializeField, ShowIf("IsHeal")]
    public int HealAmount { get; private set; }
    
    [field: SerializeField]
    public bool IsAOE { get; private set; }



    [field: SerializeField]
    public bool IsAddsEffect { get; private set; }
    [field: SerializeField, ShowIf("IsAddsEffect")]
    public EffectType Effect { get; private set; }
    [field: SerializeField, ShowIf("IsAddsEffect")]
    public int EffectValue { get; private set; }
    [field: SerializeField, ShowIf("IsAddsEffect")]
    public int EffectDuration { get; private set; }


    public int CalculateDamage(Character user, Character target)
    {
        return Mathf.Min(Damage, target.Health);
    }

    public int CalculateHeal(Character user, Character target)
    {
        return Mathf.Min(HealAmount, target.MaxHealth - target.Health);
    }



    [System.Serializable]
    public struct ConsiderationDefinition
    {
        public ConsiderationType Type;
        public AnimationCurve Curve;
    }

    [field: SerializeField]
    public List<ConsiderationDefinition> Considerations { get; private set; }
}
