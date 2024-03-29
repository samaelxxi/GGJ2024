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
            _team1[i].Name = $"{_team1[i]._data.name}";
        }

        _team2 = new Character[data.Team2.Count];
        for (int i = 0; i < data.Team2.Count; i++)
        {
            _team2[i] = new Character(data.Team2[i], 1);
            _team2[i].InitAI(this);
            _team2[i].Name = $"{_team1[i]._data.name}";
        }

        _turnOrder.AddRange(_team1);
        _turnOrder.AddRange(_team2);
        _turnOrder = _turnOrder.OrderByDescending(x => x._data.Initiative).ToList();
    }

    public int GetCharacterTeam(Character character)
    {
        return character.Team;
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
        string debug = "";
        foreach (var character in _team1)
            debug += $"{character.Name}({character.Health}) ";
        debug += " | ";
        foreach (var character in _team2)
            debug += $"{character.Name}({character.Health}) ";
        Debug.Log(debug);

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

        if (_turnOrder.All(x => GetCharacterTeam(x) == 0))
        {
            Debug.Log("Team 0 wins");
            Events.CombatEnd(0);
        }
        else if (_turnOrder.All(x => GetCharacterTeam(x) == 1))
        {
            Debug.Log("Team 1 wins");
            Events.CombatEnd(1);
        }
        else
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


        Events.SkillUsed(user, skill, CollectSkillTargets(user, skill, target));

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
            var effect = new Effect(skill.Effect, skill.EffectValue, skill.EffectDuration);
            if (skill.Effect == EffectType.AllyDefense)
                effect.SetOwner(user);
            target.AddEffect(effect);
        }

        EndTurn();
    }

    List<Character> CollectSkillTargets(Character user, Skill skill, Character target)
    {
        HashSet<Character> targets = new();

        if (skill.IsAOE)
        {
            foreach (var character in _turnOrder)
            {
                if ((skill.IsAttack && GetCharacterTeam(character) != GetCharacterTeam(user)) ||
                    (skill.IsHeal   && GetCharacterTeam(character) == GetCharacterTeam(user)))
                        targets.Add(character);
            }
        }
        else
        {
            targets.Add(target);
        }

        if (skill.IsAddsEffect)
        { // TODO AOE effects?
            targets.Add(target);
        }

        return targets.ToList();
    }

    public bool IsSkillUsageCorrect(Character user, Skill skill, Character target)
    {
        if (skill.Target == SkillTarget.Enemy && user.Team == target.Team)
            return false;
        if (skill.Target == SkillTarget.MyTeam && user.Team != target.Team)
            return false;
        if (skill.Target == SkillTarget.Ally && (user.Team != target.Team || user == target))
            return false;
        if (skill.Target == SkillTarget.Self && user != target)
            return false;
        // if (skill.IsSelfOnly && user != target)
        //     return false;
        // if (skill.IsAOE && target != null)
        //     return false;
        return true; 
    }
}
