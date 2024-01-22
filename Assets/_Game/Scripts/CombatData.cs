using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombatData", menuName = "Data/CombatData")]
public class CombatData : ScriptableObject
{
    [field: SerializeField]
    public List<CharacterData> Team1 { get; private set; }

    [field: SerializeField]
    public List<CharacterData> Team2 { get; private set; }
}
