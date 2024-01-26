using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersRegistry", menuName = "CharactersRegistry", order = 1)]

[Serializable]
public class CharViewData
{
    public GameObject prefab;
    public Sprite UIIcon;
}

public class CharactersRegistry : Registry<CharacterData, CharViewData>
{

}
