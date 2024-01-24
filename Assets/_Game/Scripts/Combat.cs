using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Combat
{
    public Character[] _team1;
    public Character[] _team2;

    List<Character> _turnOrder = new();


    public CombatEvents Events = new();


    public void Init(CombatData data)
    {
        _team1 = new Character[data.Team1.Count];
        for (int i = 0; i < data.Team1.Count; i++)
        {
            _team1[i] = new Character(data.Team1[i], 0);
            _team1[i].InitAI(this);  // TODO comment
            _team1[i].Name = $"Team1_{i}";
        }

        _team2 = new Character[data.Team2.Count];
        for (int i = 0; i < data.Team2.Count; i++)
        {
            _team2[i] = new Character(data.Team2[i], 1);
            _team2[i].InitAI(this);
            _team2[i].Name = $"Team2_{i}";
        }

        _turnOrder.AddRange(_team1);
        _turnOrder.AddRange(_team2);
        _turnOrder = _turnOrder.OrderByDescending(x => x._data.Initiative).ToList();
    }

    public int GetCharacterTeam(Character character)
    {
        return character.Team;

        if (_team1.Contains(character))
            return 0;
        else if (_team2.Contains(character))
            return 1;
        else
            Debug.LogError($"Character {character.Name} not found in any team");
        return -1;
    }

    public void StartCombat()
    {
        StartNewTurn();
    }

    public IEnumerable<Character> GetEnemies(Character character)
    {
        foreach (var enemy in _turnOrder)
        {
            if (GetCharacterTeam(enemy) != GetCharacterTeam(character))
                yield return enemy;
        }
    }

    public Character GetAnyEnemy(Character character)
    {
        foreach (var enemy in _turnOrder)
        {
            if (GetCharacterTeam(enemy) != GetCharacterTeam(character))
                return enemy;
        }
        return null;
    }

    public IEnumerable<Character> GetFriends(Character character)
    {
        foreach (var friend in _turnOrder)
        {
            if (GetCharacterTeam(friend) == GetCharacterTeam(character))
                yield return friend;
        }
    }

    public Character GetAnyFriend(Character character)
    {
        foreach (var friend in _turnOrder)
        {
            if (GetCharacterTeam(friend) == GetCharacterTeam(character))
                return friend;
        }
        return null;
    }

    void StartNewTurn()
    {
        _turnOrder[0].OnTurnStart();
        Events.CharacterGetsTurn(_turnOrder[0]);
    }

    public void MakeNextAITurn()
    {
        var action = _turnOrder[0].GetAITurn();
        UseSkill(action.Context.User, action.Context.Skill, action.Context.Target);
    }

    void EndTurn()
    {
        Character character = _turnOrder[0];
        _turnOrder.RemoveAt(0);
        _turnOrder.Add(character);
        _turnOrder.RemoveAll(x => x.IsDead);

        StartNewTurn();
    }

    public void UseSkill(Character user, Skill skill, Character target)
    {
        if (!IsSkillUsageCorrect(user, skill, target))
        {
            Debug.LogError($"Skill usage is incorrect: {skill.name} by {user.Name} on {target.Name}");
            return;
        }
        Debug.Log($"Skill usage: {skill.name} by {user.Name} on {target.Name}");

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
            target.AddEffect(new Effect(skill.Effect, skill.EffectValue, skill.EffectDuration));
        }

        EndTurn();
    }

    public bool IsSkillUsageCorrect(Character user, Skill skill, Character target)
    {
        if (skill.IsAttack && user.Team == target.Team)
            return false;
        if (skill.IsHeal && user.Team != target.Team)
            return false;
        // if (skill.IsAOE && target != null)
        //     return false;
        return true; 
    }
}
