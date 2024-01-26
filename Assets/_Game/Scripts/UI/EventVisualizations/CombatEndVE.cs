using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEndVE : VisualEvent
{

    int _victoryTeamId;

    public CombatEndVE(int victoryTeamId) 
    {
        _victoryTeamId = victoryTeamId;
    }
    public override IEnumerator Display()
    {
        Game.Instance.UIView.UiManager.DisplayCombatEnd(_victoryTeamId == 0);
        yield return null;
    }
}
