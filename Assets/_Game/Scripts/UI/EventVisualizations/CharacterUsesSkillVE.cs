using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterUsesSkillVE : VisualEvent
{
    CharacterView _user;
    List<CharacterView> _targets;
    List<VisualEvent> _attachedEvents = new List<VisualEvent>();
    Skill _skill;

    public bool IsAttack => _skill.IsAttack;

    public MonoBehaviour CourutineOvner;
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
        
        DramatickLightOn();
        _user.DisplayeSkill(_skill, _targets);
        yield return new WaitForSeconds(0.3f);
        foreach(var attachedEvent in _attachedEvents)
        {
            CourutineOvner.StartCoroutine(attachedEvent.Display());
        }
        yield return new WaitForSeconds(0.7f);

        while(_user.InActiveAnimation){
            yield return null; // skip frame
        }
        DramatickLightOff();
        
    }

    void DramatickLightOn(){
        Game.Instance.UIView.DramaticShade.enabled = true;
        _user.transform.localPosition = new Vector3(0,0, -1);
        foreach(var cv in _targets)
        {
            cv.transform.localPosition = new Vector3(0,0, -1);
        }
    }

    void DramatickLightOff()
    {
        _user.transform.localPosition = Vector3.zero;
         foreach(var cv in _targets)
        {
            cv.transform.localPosition = Vector3.zero;
        }
        Game.Instance.UIView.DramaticShade.enabled = false;
    }
    
        public bool AttachVisualEvent(VisualEvent attachedEvent)
        {
            if((_skill.IsAttack && attachedEvent is CharacterDamagedVisualEvent) || (_skill.IsHeal && attachedEvent is CharacterHealedVE)
                /** || (_skill.IsAddsEffect && attachedEvent ) **/
                )
            {
                _attachedEvents.Add(attachedEvent);
                return true;
            } 
            return false;

        }

 
}
