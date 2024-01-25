using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamagedVisualEvent : VisualEvent
{
    CharacterView _characterView;
    int _damage;

    public CharacterDamagedVisualEvent(Character character, int damage)
    {
        _damage = damage;
        _characterView = Game.Instance.UIView.GetViewByCharacter(character);
    }

    public override IEnumerator Display()
    {
        _characterView.DisplayTakeDamage();
        while(_characterView.InActiveAnimation){
            yield return null; // skip frame
        }

    }
}
