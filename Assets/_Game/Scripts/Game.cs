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

    public override void Awake()
    {
        base.Awake();
        _combat = new Combat();
        _combat.Init(_combatDatas[_currentCombatIndex]);
        _uiView.Init(_combat);
    }
    void Start()
    {
        
        // _combat.Events.OnCharacterGetsTurn += MakeNextTurn;  // for auto combat or smth
        // _combat.Events.OnCombatEnd += OnCombatEnd;
        _combat.StartCombat();
    }

    void Update()
    {
        
    }


    int _madeTurns = 0;
    bool _combatEnded = false;

    void OnCombatEnd(int team) { _combatEnded = true;}

    void MakeNextTurn(Character character)
    {
        Debug.Log($"Character {character.Name} made turn");
        _madeTurns++;
        if (_combatEnded)
        {
            return;
        }
        else
        {
            _combat.MakeNextAITurn();
        }
    }
}
