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
        // action.AddConsideration(new ConsiderationRandom());
        if (skill.IsAttack)
        {
            ResponseCurve curve = new(ResponseCurve.CurveType.Linear);
            curve.SetInputRange(0, 30);
            var consideration = new ConsiderationDamage();
            consideration.SetResponseCurve(curve);
            action.AddConsideration(consideration);

            ResponseCurve curve2 = new(ResponseCurve.CurveType.Linear, slope: 0.5f, yshift: 1);
            curve2.SetInputRange(0, 3);
            var consideration2 = new ConsiderationKill();
            consideration2.SetResponseCurve(curve2);
            action.AddConsideration(consideration2);

            ResponseCurve curve3 = new(ResponseCurve.CurveType.Logistic, slope: 1f, exponent: -1, yshift: 0.2f);
            curve3.SetInputRange(0, 1);
            var consideration3 = new ConsiderationTargetHPPercentage();
            consideration3.SetResponseCurve(curve3);
            action.AddConsideration(consideration3);
        }
        else if (skill.IsHeal)
        {
            ResponseCurve curve = new(ResponseCurve.CurveType.Linear);
            curve.SetInputRange(0, 30);
            var consideration = new ConsiderationHeal();
            consideration.SetResponseCurve(curve);
            action.AddConsideration(consideration);

            ResponseCurve curve3 = new(ResponseCurve.CurveType.Logistic, slope: 1f, exponent: -1, yshift: 0.2f);
            curve3.SetInputRange(0, 1);
            var consideration3 = new ConsiderationTargetHPPercentage();
            consideration3.SetResponseCurve(curve3);
            action.AddConsideration(consideration3);
        }
    }

    public AIAction ChooseBestAction()
    {
        AIAction bestAction = null;
        float minScore = 0;

        string debugStr = "";

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
                debugStr += $"{concreteAction.debugStr}\n";
            }
        }
        debugStr += $"Best action: {bestAction.Context.Skill.Name} - {bestAction.Context.Target.Name}\n";
        Debug.Log(debugStr);

        return bestAction;
    }
}
