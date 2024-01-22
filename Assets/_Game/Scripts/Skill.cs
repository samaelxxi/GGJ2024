using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[CreateAssetMenu(fileName = "Skill", menuName = "Skill", order = 1)]
public class Skill : ScriptableObject
{
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
    public Effect Effect { get; private set; }
}
