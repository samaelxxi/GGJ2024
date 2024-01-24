using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] TextMeshProUGUI SkillName;
    public Skill Skill;


    bool _isSelected = false;
    public bool IsSelected {
        get => _isSelected;
        set {
            _isSelected = value;
            transform.localScale = _isSelected ? new Vector3(1.2f, 1.2f, 1) : Vector3.one;
        }
    }

    public void Init(Skill skill){
        Skill = skill;
        SkillName.text = Skill.name;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Game.Instance.UIView.SelectSkillBtn(this);
        transform.localScale = new Vector3(1.2f, 1.2f, 1);
        IsSelected = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsSelected) transform.localScale = new Vector3(1.1f, 1.1f, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!IsSelected) transform.localScale = Vector3.one;
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
