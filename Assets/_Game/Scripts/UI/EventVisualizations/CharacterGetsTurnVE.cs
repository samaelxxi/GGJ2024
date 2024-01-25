using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CharacterGetsTurnVE : VisualEvent
{
    public CharacterView CharacterView => _characterView;
    CharacterView _characterView;
    public CharacterGetsTurnVE(Character character){
        _characterView = Game.Instance.UIView.GetViewByCharacter(character);
    }
    public override IEnumerator Display()
    {
        yield return null;
    }
}
