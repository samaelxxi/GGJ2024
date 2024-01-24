using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    [SerializeField] SkillBtn SkillBtnPrefab;
    public void ShowSkills(List<Skill> skills)
    {

        foreach(Skill skill in skills)
        {
            Debug.Log(skill.name);
            SkillBtn newBtn = Instantiate(SkillBtnPrefab, transform);
            newBtn.Init(skill);
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
