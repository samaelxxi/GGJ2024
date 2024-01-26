using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAction : AIAction
{
    public Skill Skill;
    public Character User;

    public SkillAction() {}
    public SkillAction(AIAction action, AIContext context) : base(action, context) {}



    public override IEnumerable<AIAction> GenerateConcreteActions(Combat combat)
    {
        if (Skill.Target == SkillTarget.Self)
        {
            var action = new SkillAction(this, new AIContext
            {
                User = User,
                Skill = Skill,
                Target = User
            });
            yield return action;
        }
        else if (Skill.Target == SkillTarget.Enemy)
        {
            foreach (var target in combat.GetEnemies(User))
            {
                var action = new SkillAction(this, new AIContext
                {
                    User = User,
                    Skill = Skill,
                    Target = target
                });
                yield return action;
            }
        }
        else if (Skill.Target == SkillTarget.Ally)
        {
            foreach (var target in combat.GetFriends(User))
            {
                if (target == User)
                    continue;
                var action = new SkillAction(this, new AIContext
                {
                    User = User,
                    Skill = Skill,
                    Target = target
                });
                yield return action;
            }
        }
        else if (Skill.Target == SkillTarget.MyTeam)
        {
            foreach (var target in combat.GetFriends(User))
            {
                var action = new SkillAction(this, new AIContext
                {
                    User = User,
                    Skill = Skill,
                    Target = target
                });
                yield return action;
            }
        }
    }
}
