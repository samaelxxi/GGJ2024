using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData", order = 1)]
public class CharacterData : ScriptableObject
{
    [field: SerializeField]
    public int TotalHealth { get; private set; }

    [field: SerializeField]
    public int Initiative { get; private set; }


    [field: SerializeField]
    public List<Skill> Skills { get; private set; }
}
