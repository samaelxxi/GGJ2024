using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersRegistry", menuName = "CharactersRegistry", order = 1)]

public class CharactersRegistry : Registry<CharacterData, GameObject>
{
}
