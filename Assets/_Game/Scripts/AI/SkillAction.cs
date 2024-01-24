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
        // Debug.Log($"{Skill}");
        if (Skill.IsAOE)
        {
            var action = new SkillAction(this, new AIContext
            {
                User = User,
                Skill = Skill,
                Target = Skill.IsAttack ? combat.GetAnyEnemy(User) : combat.GetAnyFriend(User)
            });
            yield return action;
        }
        else if (Skill.IsAttack)
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
        else if (Skill.IsHeal)
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
