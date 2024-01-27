using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    UICharacterCard _uiCharCard;
    [SerializeField] Transform _body;
    [SerializeField] Transform _projectileSpawnPos;
    public Transform ProjectileHit;

    [SerializeField] Projectile ProjectilePrefab;

    [SerializeField] SelectionMarker selectionMarker;

    [SerializeField] AudioClip DeathSound;

    public Character Character { get; set; }

    Animator _animator;

    [HideInInspector] 
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
        _uiCharCard.SetHealthValue((float)Character.Health / Character._data.TotalHealth);
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


    public void Init(Character character, UICharacterCard card)
    {
        Character = character;
        if(Character.Team != 0) _body.localScale = new Vector3(-1, 1, 1);
        _uiCharCard = card;
    }

    public void Highlight()
    {
        _isHighlighted = true;
        selectionMarker.SetType(Character.Team == 0 ? SelectionMarker.Type.Ally : SelectionMarker.Type.Enemy);
    }

    List<CharacterView> _targetsForDisplaingSkill;
    public virtual void DisplayeSkill(Skill skill, List<CharacterView> _targets)
    {
        _targetsForDisplaingSkill = _targets;
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
        Destroy(_uiCharCard.gameObject);
        AudioSource.PlayClipAtPoint(DeathSound, Vector3.zero);
        GetComponent<Collider2D>().enabled = false;
        selectionMarker.SetVisible(false);
        _animator.SetTrigger("Die");
        InActiveAnimation = true;
    }

    public void Shoot()
    {
        foreach (CharacterView target in _targetsForDisplaingSkill)
        {
            Projectile projectile = Instantiate(ProjectilePrefab, _projectileSpawnPos.position, Quaternion.identity);
            projectile.Init(target.ProjectileHit.position);
        }
    }

}
