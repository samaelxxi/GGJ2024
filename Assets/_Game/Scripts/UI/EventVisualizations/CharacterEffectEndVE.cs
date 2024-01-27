using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectEndVE : VisualEvent
{
    CharacterView _characterView;
    Effect _effect;
    public CharacterEffectEndVE(Character character, Effect effect)
    {
        _characterView = Game.Instance.UIView.GetViewByCharacter(character);
        _effect = effect;
    }
    public override IEnumerator Display()
    {
        _characterView.SetShieldEffectVisible(false);
        yield return null;
    }


}
