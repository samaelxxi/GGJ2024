using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns.Singleton;

public class Game : Singleton<Game>
{
    public CombatEvents Events => _combat.Events;

    Combat _combat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
