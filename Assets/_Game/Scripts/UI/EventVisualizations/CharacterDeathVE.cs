using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterDeathVE : VisualEvent
{
    CharacterView _characterView;
    public CharacterDeathVE(Character character)
    {
        _characterView = Game.Instance.UIView.GetViewByCharacter(character);
    }
    public override IEnumerator Display()
    {
        _characterView.DisplayDeath();
       
        yield return new WaitForSeconds(1); // skip frame
    }

    
}
