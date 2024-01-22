using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns.Singleton;


public class Game : Singleton<Game>
{
    public CombatEvents Events => _combat.Events;
    [SerializeField] UIView _uiView;

    Combat _combat;

    void Start()
    {
        _combat = new Combat();
        _uiView.Init(_combat);
    }


    void Update()
    {
        
    }
}
