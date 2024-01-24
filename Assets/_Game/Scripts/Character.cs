using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character
{
    public CharacterData _data;

    public string Name;

    List<Effect> _effects;

    int _health;
    bool _isDead;
    int _team = -1;


    public bool IsDead => _isDead;
    public int Health => _health;
    public int MaxHealth => _data.TotalHealth;
    public int Team => _team;


    AIBrain _aiBrain;



    public Character(CharacterData data, int team = -1)
    {
        _data = data;
        _health = data.TotalHealth;
        _effects = new List<Effect>();
        _team = team;
    }

    public void InitAI(Combat combat)
    {
        _aiBrain = new AIBrain(combat);

        foreach (var skill in _data.Skills)
            _aiBrain.AddSkill(skill, this);
    }

    public AIAction GetAITurn()
    {
        return _aiBrain.ChooseBestAction();
    }

    public void GetDamage(int damage)
    {
        Debug.Log($"Character {Name} gets {damage} damage");

        var def = _effects.FirstOrDefault(e => e.Type == EffectType.Defense);
        if (def != null)
        {
            if (def.Amount >= damage)
            {
                def.Amount -= damage;
                Game.Instance.Events.CharacterDamaged(this, damage);
                // Game.Instance.Events.OnCharacterDamaged(this, damage);
                return;
            }
            else
            {
                damage -= def.Amount;
                _effects.Remove(def);
                Game.Instance.Events.CharacterEffectEnd(this, def);
            }
        }

        _health -= damage;

        if (_health <= 0)
        {
            Debug.Log($"Character {Name} is dead");
            _health = 0;
            _isDead = true;
            Game.Instance.Events.CharacterDied(this);
        }
        else
        {
            Game.Instance.Events.CharacterDamaged(this, damage);
        }
    }

    public void GetHeal(int heal)
    {
        Debug.Log($"Character {Name} gets {heal} heal");

        int oldHp = _health;
        _health += heal;
        if (_health > _data.TotalHealth)
        {
            _health = _data.TotalHealth;
        }
        Game.Instance.Events.CharacterHealed(this, _health - oldHp);
    }

    public void AddEffect(Effect effect)
    {
        Debug.Log($"Character {Name} gets effect {effect.Type} {effect.Amount} {effect.Duration}");

        _effects.Add(effect);
        Game.Instance.Events.CharactersGetsEffect(this, effect);
    }

    public void OnTurnStart()
    {
        foreach (var effect in _effects)
        {
            effect.Duration--;
            if (effect.Duration <= 0)
            {
                _effects.Remove(effect);
                Game.Instance.Events.CharacterEffectEnd(this, effect);
            }
        }
    }
}
