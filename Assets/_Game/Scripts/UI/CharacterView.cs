using Unity.VisualScripting;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] SpriteHealthbar Healthbar;
    [SerializeField] Transform _body;
    [SerializeField] SelectionMarker selectionMarker;

    public Character Character { get; set; }

    Animator _animator;

    public bool InActiveAnimation = false;

    bool _isHighlighted = false;
    bool _isSelected = false;
    public bool IsSelected 
    {
        get => _isSelected;
        set  
        {
            _isSelected = value;
            selectionMarker.SetVisible(value);
            selectionMarker.SetType( value ? SelectionMarker.Type.Selection : Character.Team == 0 ? SelectionMarker.Type.Ally : SelectionMarker.Type.Enemy);
        }
    }
    // Start is called before the first frame update
    public void UpdateStatus()
    {
        Healthbar.SetValue((float)Character.Health / Character._data.TotalHealth);
    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        selectionMarker.SetType(Character.Team == 0 ? SelectionMarker.Type.Ally : SelectionMarker.Type.Enemy);
        selectionMarker.SetVisible(false);
        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (Character.IsDead) return;
        selectionMarker.SetVisible(_isHighlighted || IsSelected);
        if(IsSelected && !_isHighlighted)  selectionMarker.SetType(SelectionMarker.Type.Selection);
        _isHighlighted = false;
    }


    public void Init(Character character)
    {
        Character = character;
        // set sprite and other stuff
    }

    public void Highlight()
    {
        _isHighlighted = true;
        selectionMarker.SetType(Character.Team == 0 ? SelectionMarker.Type.Ally : SelectionMarker.Type.Enemy);
    }

    public virtual void DisplayeSkill(Skill skill)
    {
        if (skill.IsAddsEffect)
        {
            _animator.SetTrigger("SpecialAction1");
        }
        else
        {
            _animator.SetTrigger("Attack");
        }
        InActiveAnimation = true;
    }

    public void DisplayTakeDamage()
    {
        UpdateStatus();
        _animator.SetTrigger("TakeDamage");
        InActiveAnimation = true;
    }

    public void DisplayDeath()
    {
        Destroy(Healthbar.gameObject);
        _body.GetComponent<Collider2D>().enabled = false;
        _animator.SetTrigger("Die");
        InActiveAnimation = true;
    }

}
