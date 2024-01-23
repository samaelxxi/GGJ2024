using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character
{
    public CharacterData _data;

    List<Effect> _effects;

    int _health;
    bool _isDead;

    public bool IsDead => _isDead;
    public int Health => _health;


    public Character(CharacterData data)
    {
        _data = data;
        _health = data.TotalHealth;
        _effects = new List<Effect>();
    }

    public void GetDamage(int damage)
    {
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
