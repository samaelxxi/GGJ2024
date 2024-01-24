using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain
{
    List<AIAction> _actions = new();
    Combat _combat;

    public AIBrain(Combat combat)
    {
        _combat = combat;
    }

    public void AddSkill(Skill skill, Character user)
    {
        // Debug.Log($"Add skill {skill}");
        var action = new SkillAction
        {
            Skill = skill,
            User = user
        };
        _actions.Add(action);
    }

    public AIAction ChooseBestAction()
    {
        AIAction bestAction = null;
        float minScore = 0;

        foreach (var abstractAction in _actions)
        {
            SkillAction skillAction = abstractAction as SkillAction;
            // Debug.Log($"Abstract action {skillAction.Skill}");
            foreach (var concreteAction in abstractAction.GenerateConcreteActions(_combat))
            {
                var curScore = concreteAction.Score(_combat);
                if (curScore > minScore)
                {
                    minScore = curScore;
                    bestAction = concreteAction;
                }
            }
        }

        return bestAction;
    }
}
