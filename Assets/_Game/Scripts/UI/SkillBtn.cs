
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField] TextMeshProUGUI SkillName;
    [SerializeField] Image Background;
    [HideInInspector] public Skill Skill;

    [SerializeField] AudioClip PointerOn;
    [SerializeField] AudioClip Selected;
    [SerializeField] AudioClip Deselected;


    bool _isSelected = false;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            if(_isSelected)
            {
                transform.localScale = new Vector3(1.2f, 1.2f, 1);
                AudioSource.PlayClipAtPoint(Selected, Vector3.zero);

            } else 
            {
                transform.localScale =  Vector3.one;
                AudioSource.PlayClipAtPoint(Deselected, Vector3.zero);
            }
        }
    }

    public void Init(Skill skill, Sprite backgroundSprite)
    {
        Skill = skill;
        SkillName.text = Skill.name;
        Background.sprite = backgroundSprite;

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Game.Instance.UIView.SelectSkillBtn(this);
        IsSelected = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            transform.localScale = new Vector3(1.1f, 1.1f, 1);
            AudioSource.PlayClipAtPoint(PointerOn, Vector3.zero);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsSelected)
        {
            transform.localScale = Vector3.one;
        }
    }
}
