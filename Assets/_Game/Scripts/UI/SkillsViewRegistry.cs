using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillsViewRegistry", menuName = "SkillsViewRegistry", order = 1)]

[Serializable]
public class SkillViewData
{
    public Sprite CardSprite;
    public GameObject TargetEffect;

}

public class SkillsViewRegistry : Registry<Skill, SkillViewData> { }
