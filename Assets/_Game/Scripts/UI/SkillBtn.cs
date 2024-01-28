
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
                AudioSource.PlayClipAtPoint(Selected, new Vector3(0, 0, -10));

            } else 
            {
                transform.localScale =  Vector3.one;
                AudioSource.PlayClipAtPoint(Deselected, new Vector3(0, 0, -10));
            }
        }
    }

    public void Init(Skill skill, Sprite backgroundSprite, string skillDisplayName)
    {
        Skill = skill;
        SkillName.text = skillDisplayName;
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
            AudioSource.PlayClipAtPoint(PointerOn, new Vector3(0, 0, -10));
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
