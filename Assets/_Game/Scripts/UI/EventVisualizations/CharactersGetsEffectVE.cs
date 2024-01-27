using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersGetsEffectVE : VisualEvent
{

    CharacterView _characterView;
    Effect _effect;
    public CharactersGetsEffectVE(Character character, Effect effect)
    {
        _characterView = Game.Instance.UIView.GetViewByCharacter(character);
        _effect = effect;
    }
    public override IEnumerator Display()
    {
        _characterView.SetShieldEffectVisible(true);
        yield return null;
    }
}
