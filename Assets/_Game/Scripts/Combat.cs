using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Combat
{
    Character[] _team1;
    Character[] _team2;

    List<Character> _turnOrder = new();

    public CombatEvents Events = new();


    public void Init(CombatData data)
    {
        _team1 = new Character[data.Team1.Count];
        for (int i = 0; i < data.Team1.Count; i++)
        {
            _team1[i] = new Character(data.Team1[i]);
        }

        _team2 = new Character[data.Team2.Count];
        for (int i = 0; i < data.Team2.Count; i++)
        {
            _team2[i] = new Character(data.Team2[i]);
        }

        _turnOrder.AddRange(_team1);
        _turnOrder.AddRange(_team2);
        _turnOrder = _turnOrder.OrderByDescending(x => x._data.Initiative).ToList();
    }

    public int GetCharacterTeam(Character character)
    {
        if (_team1.Contains(character))
            return 0;
        else if (_team2.Contains(character))
            return 1;
        else
            Debug.LogError("Character not found in any team");
        return -1;
    }

    public void StartCombat()
    {
        StartNewTurn();
    }

    void StartNewTurn()
    {
        _turnOrder[0].OnTurnStart();
        Events.CharacterGetsTurn(_turnOrder[0]);
    }

    void EndTurn()
    {
        Character character = _turnOrder[0];
        _turnOrder.RemoveAt(0);
        _turnOrder.Add(character);
        _turnOrder.RemoveAll(x => x.IsDead);
    }

    public void UseSkill(Character user, Skill skill, Character target=null)
    {
        if (!IsSkillUsageCorrect(user, skill, target))
        {
            Debug.LogError($"Skill usage is incorrect: {skill.name} by {user._data.name} on {target._data.name}");
            return;
        }

        if (skill.IsAttack)
        {
            if (skill.IsAOE)
            {
                foreach (var character in _turnOrder)
                {
                    if (GetCharacterTeam(character) == GetCharacterTeam(user))
                        continue;
                    Events.CharacterAttacks(user, character);
                    character.GetDamage(skill.Damage);
                }
            }
            else
            {
                Events.CharacterAttacks(user, target);
                target.GetDamage(skill.Damage);
            }
        }
        else if (skill.IsHeal)
        {
            if (skill.IsAOE)
            {
                foreach (var character in _turnOrder)
                {
                    if (GetCharacterTeam(character) != GetCharacterTeam(user))
                        continue;
                    Events.CharacterHeals(user, character);
                    character.GetHeal(skill.HealAmount);
                }
            }
            else
            {
                Events.CharacterHeals(user, target);
                target.GetHeal(skill.HealAmount);
            }
        }

        if (skill.IsAddsEffect)
        { // TODO AOE effects?
            target.AddEffect(skill.Effect);
        }

        EndTurn();
    }

    public bool IsSkillUsageCorrect(Character user, Skill skill, Character target=null)
    {
        if (skill.IsAttack && GetCharacterTeam(user) == GetCharacterTeam(target))
            return false;
        if (skill.IsHeal && GetCharacterTeam(user) != GetCharacterTeam(target))
            return false;
        if (skill.IsAOE && target != null)
            return false;
        return true; 
    }
}
