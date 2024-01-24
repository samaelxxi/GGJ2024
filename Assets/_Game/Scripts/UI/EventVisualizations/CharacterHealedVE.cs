using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealedVE : VisualEvent
{
     CharacterView _characterView;
    int _heal;

    public CharacterHealedVE(Character character, int heal)
    {
        _heal = heal;
        _characterView = Game.Instance.UIView.GetViewByCharacter(character);
    }

    public override IEnumerator Display()
    {
         // Spawn particles?
        _characterView.UpdateStatus();
        yield return new WaitForSeconds(0.5f);
    }
}
