using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillsViewRegistry", menuName = "SkillsViewRegistry", order = 1)]

public class SkillsViewRegistry : ScriptableObject
{
    [Serializable]
    class Entry
    {
        public Skill Skill;
        public Sprite Sprite;
    }

    public Sprite GetSprite(Skill skill)
    {

        if(Registry[skill] == null)
        {
            Debug.LogWarning($"No sprite found for {skill.name}");
        }
        return Registry[skill];
    }

    public Dictionary<Skill, Sprite> Registry {
        get {
            if(_registry == null)
            {
                _registry = new Dictionary<Skill, Sprite>();
                foreach(var entry in RegistryList)
                {
                    _registry.Add(entry.Skill, entry.Sprite);
                }
            }
            return _registry;
        }
    }
    Dictionary<Skill, Sprite> _registry;

    [field: SerializeField]
    private List<Entry> RegistryList;
}
