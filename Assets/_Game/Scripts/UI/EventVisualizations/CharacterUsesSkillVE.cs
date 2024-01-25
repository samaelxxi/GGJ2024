using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterUsesSkillVE : VisualEvent
{
    CharacterView _user;
    List<CharacterView> _targets;
    Skill _skill;
    public CharacterUsesSkillVE(Character user, Skill skill, List<Character> targets)
    {
        _user = Game.Instance.UIView.GetViewByCharacter(user);
        _targets = new List<CharacterView>(targets.Count);
        foreach(var t in targets)
        {
            _targets.Add(Game.Instance.UIView.GetViewByCharacter(t));
        }
        _skill = skill;
    }
    public override IEnumerator Display()
    {
        Game.Instance.UIView.DramaticShade.enabled = true;
        _user.transform.localPosition = new Vector3(0,0, -1);
        foreach(var cv in _targets)
        {
            cv.transform.localPosition = new Vector3(0,0, -1);
        }
        yield return new WaitForSeconds(1);
        _user.transform.localPosition = Vector3.zero;
         foreach(var cv in _targets)
        {
            cv.transform.localPosition = Vector3.zero;
        }
        Game.Instance.UIView.DramaticShade.enabled = false;


    }
    
 
}
