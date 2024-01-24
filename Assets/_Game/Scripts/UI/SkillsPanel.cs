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
            newBtn.Init(skill, SkillSpritesRegistry.GetSprite(skill));
        }
    }

    public void Hide(){
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
