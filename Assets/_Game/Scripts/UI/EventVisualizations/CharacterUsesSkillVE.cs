using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUsesSkillVE : VisualEvent
{
    CharacterView _user;
    CharacterView _target;
    Skill _skill;

    Skill skill;

    public CharacterUsesSkillVE(Character user, Skill skill, Character target)
    {
        _user = Game.Instance.UIView.GetViewByCharacter(user);
        _target = Game.Instance.UIView.GetViewByCharacter(target);
        _skill = skill;
    }
    public override IEnumerator Display()
    {
        Game.Instance.UIView.DramaticShade.enabled = true;
        
        yield return null;
    }
    
 
}
