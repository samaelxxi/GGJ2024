using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns.Singleton;


public class Game : Singleton<Game>
{
    public CombatEvents Events => _combat.Events;
    public UIView UIView=> _uiView;
    [SerializeField] UIView _uiView;


    [SerializeField] List<CombatData> _combatDatas;
    int _currentCombatIndex = 0;


    Combat _combat;

    void Start()
    {
        _combat = new Combat();
        _combat.Init(_combatDatas[_currentCombatIndex]);
        _uiView.Init(_combat);
        // _combat.Events.OnCharacterGetsTurn += MakeNextTurn;  // for auto combat or smth

        _combat.StartCombat();

    }

    void Update()
    {
        
    }


    int _madeTurns = 0;
    void MakeNextTurn(Character character)
    {
        Debug.Log($"Character {character.Name} made turn");
        _madeTurns++;
        if (_madeTurns >= 10)
        {
            return;
        }
        else
        {
            _combat.MakeNextAITurn();
        }
    }
}
