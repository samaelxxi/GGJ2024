using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public abstract class Consideration
{
    protected Response _responseCurve;

    public void SetResponseCurve(Response curve)
    {
        _responseCurve = curve;
    }

    public abstract float Score(Combat combat, AIContext context);
}


public class ConsiderationRandom : Consideration
{
    public override float Score(Combat combat, AIContext context)
    {
        return Random.Range(0f, 1f);
    }
}

public class ConsiderationDamage : Consideration
{
    public override float Score(Combat combat, AIContext context)
    {
        if (!context.Skill.IsAttack)
            return 0;

        if (context.Skill.IsAOE)
        {
            int totalDamage = 0;
            foreach (var target in combat.GetEnemies(context.User))
            {
                var damage = context.Skill.CalculateDamage(context.User, target);
                totalDamage += damage;
            }
            return _responseCurve.ComputeValue(totalDamage);
        }
        else
        {
            var damage = context.Skill.CalculateDamage(context.User, context.Target);
            return _responseCurve.ComputeValue(damage);
        }
    }
}

public class ConsiderationHeal : Consideration
{
    public override float Score(Combat combat, AIContext context)
    {
        if (!context.Skill.IsHeal)
            return 0;

        if (context.Skill.IsAOE)
        {
            int totalHeal = 0;
            foreach (var target in combat.GetFriends(context.User))
            {
                var heal = context.Skill.CalculateHeal(context.User, target);
                totalHeal += heal;
            }
            return _responseCurve.ComputeValue(totalHeal);
        }
        else
        {
            var heal = context.Skill.CalculateHeal(context.User, context.Target);
            return _responseCurve.ComputeValue(heal);
        }
    }
}

public class ConsiderationKill : Consideration
{
    public override float Score(Combat combat, AIContext context)
    {
        if (!context.Skill.IsAttack)
            return 0;

        int killed = 0;
        if (context.Skill.IsAOE)
        {
            foreach (var target in combat.GetEnemies(context.User))
            {
                if (context.Skill.Damage >= target.Health)
                    killed++;
            }
        }
        else
        {
            if (context.Skill.Damage >= context.Target.Health)
                killed++;
        }
        return _responseCurve.ComputeValue(killed);
    }
}

public class ConsiderationTargetHPPercentage : Consideration
{
    public override float Score(Combat combat, AIContext context)
    {
        if (context.Skill.IsAOE)
        {
            int totalHP = 0;
            int totalMaxHP = 0;
            foreach (var target in combat.GetEnemies(context.User))
            {
                totalHP += target.Health;
                totalMaxHP += target.MaxHealth;
            }
            return _responseCurve.ComputeValue(totalHP / totalMaxHP);
        }
        else
        {
            return _responseCurve.ComputeValue(context.Target.Health / context.Target.MaxHealth);
        }
    }
}