using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatEvents : MonoBehaviour
{
    public event Action<Character, int> OnCharacterDamaged;
    public event Action<Character, int> OnCharacterHealed;
    public event Action<Character> OnCharacterDied;
    public event Action<Character, Character> OnCharacterAttacks;
    public event Action<Character, Character> OnCharacterHeals;
    public event Action<Character, Effect> OnCharactersGetsEffect;
    public event Action<Character, Effect> OnCharacterEffectEnd;
    public event Action<Character> OnCharacterGetsTurn;



   public void CharacterDamaged(Character character, int heal)
    {
        OnCharacterDamaged?.Invoke(character, heal);
    }


    public void CharacterHealed(Character character, int heal)
    {
        OnCharacterHealed?.Invoke(character, heal);
    }

    public void CharacterDied(Character character)
    {
        OnCharacterDied?.Invoke(character);
    }

    public void CharacterAttacks(Character character, Character target)
    {
        OnCharacterAttacks?.Invoke(character, target);
    }

    public void CharacterHeals(Character character, Character target)
    {
        OnCharacterHeals?.Invoke(character, target);
    }

    public void CharactersGetsEffect(Character character, Effect effect)
    {
        OnCharactersGetsEffect?.Invoke(character, effect);
    }

    public void CharacterEffectEnd(Character character, Effect effect)
    {
        OnCharacterEffectEnd?.Invoke(character, effect);
    }

    public void CharacterGetsTurn(Character character)
    {
        OnCharacterGetsTurn?.Invoke(character);
    }
}
