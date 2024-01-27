using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField] SkillBtn SkillBtnPrefab;
    [SerializeField] SkillsViewRegistry SkillSpritesRegistry;
    public void ShowSkills(List<Skill> skills)
    {

        foreach(Skill skill in skills)
        {
            SkillBtn newBtn = Instantiate(SkillBtnPrefab, transform);
            newBtn.Init(skill, SkillSpritesRegistry.Get(skill).CardSprite, SkillSpritesRegistry.Get(skill).DisplaySkillName);
        }
    }

    public void Hide(){
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

}
