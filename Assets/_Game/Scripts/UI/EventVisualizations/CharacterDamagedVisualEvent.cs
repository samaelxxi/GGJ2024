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
         // Spawn particles?
        _characterView.UpdateStatus();
        yield return new WaitForSeconds(0.5f);
    }
}
