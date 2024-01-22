using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns.Singleton;


public class Game : Singleton<Game>
{
    public CombatEvents Events => _combat.Events;
    [SerializeField] UIView _uiView;


    [SerializeField] List<CombatData> _combatDatas;
    int _currentCombatIndex = 0;


    Combat _combat;

    void Start()
    {
        _combat = new Combat();
        _combat.Init(_combatDatas[_currentCombatIndex]);
        _uiView.Init(_combat);
        _combat.StartCombat();
    }


    void Update()
    {
        
    }
}
