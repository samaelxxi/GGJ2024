using System;
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
        foreach (var t in targets)
        {
            _targets.Add(Game.Instance.UIView.GetViewByCharacter(t));
        }
        _skill = skill;
    }
    public override IEnumerator Display()
    {

        SetDramatickLight(true);
        yield return new WaitForSeconds(0.3f);
        Action animCallback = null;
        var effect = Game.Instance.UIView.SkillsViewRegistry.Get(_skill).TargetEffect;
        if (effect)
        {
            animCallback = () =>
            {
                foreach (CharacterView target in _targets)
                {
                    if (target.Character.DoesHaveEffect(EffectType.AllyDefense))
                    {
                        UnityEngine.Object.Instantiate(effect, Game.Instance.UIView.GetViewByCharacter(target.Character.GetEffectOwner(EffectType.AllyDefense)).ProjectileHit);
                    }
                    else UnityEngine.Object.Instantiate(effect, target.ProjectileHit);
                };
            };
        }

        _user.DisplayeSkill(_skill, animCallback);
        yield return new WaitForSeconds(0.3f);
        foreach (var attachedEvent in _attachedEvents)
        {
            CourutineOvner.StartCoroutine(attachedEvent.Display());
        }
        yield return new WaitForSeconds(0.7f);

        while (_user.InActiveAnimation)
        {
            yield return null; // skip frame
        }
        SetDramatickLight(false);

    }

    void SetDramatickLight(bool isLightOn)
    {
        Vector3 charsPos = isLightOn ? new Vector3(0, 0, -1) : Vector3.zero;
        Game.Instance.UIView.DramaticShade.enabled = isLightOn;
        _user.transform.localPosition = charsPos;
        foreach (var cv in _targets)
        {
            cv.transform.localPosition = charsPos;
            if (cv.Character.DoesHaveEffect(EffectType.AllyDefense))
            {
                Game.Instance.UIView.GetViewByCharacter(cv.Character.GetEffectOwner(EffectType.AllyDefense)).transform.localPosition = charsPos;
            }
        }
    }

    public bool AttachVisualEvent(VisualEvent attachedEvent)
    {
        if ((_skill.IsAttack && attachedEvent is CharacterDamagedVisualEvent) || (_skill.IsHeal && attachedEvent is CharacterHealedVE)
            /** || (_skill.IsAddsEffect && attachedEvent ) **/
            )
        {
            _attachedEvents.Add(attachedEvent);
            return true;
        }
        return false;

    }


}
