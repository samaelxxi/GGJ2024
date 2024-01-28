using System;
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
    [SerializeField] GameObject ShieldEffect;

    [SerializeField] AudioClip DeathSound;
    [SerializeField] AudioClip TakeDamage;


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
            SetMarkerState(value, value ? SelectionMarker.Type.Selection : Character.Team == 0 ? SelectionMarker.Type.Ally : SelectionMarker.Type.Enemy);
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
        SetMarkerState(false, Character.Team == 0 ? SelectionMarker.Type.Ally : SelectionMarker.Type.Enemy);
        UpdateStatus();
        SetShieldEffectVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Character.IsDead) return;
        SetMarkerState(_isHighlighted || IsSelected, _isHighlighted ? Character.Team == 0 ? SelectionMarker.Type.Ally : SelectionMarker.Type.Enemy : SelectionMarker.Type.Selection);
        _isHighlighted = false;
    }


    public void Init(Character character, UICharacterCard card)
    {
        Character = character;
        if (Character.Team != 0) _body.localScale = new Vector3(-1, 1, 1);
        _uiCharCard = card;
    }

    public void Highlight()
    {
        _isHighlighted = true;
        SetMarkerState(true, Character.Team == 0 ? SelectionMarker.Type.Ally : SelectionMarker.Type.Enemy);
    }

    Action _animationEventCallback;
    public virtual void DisplayeSkill(Skill skill, Action animationEventCallback)
    {
        _animationEventCallback = animationEventCallback;
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
        AudioSource.PlayClipAtPoint(DeathSound, new Vector3(0, 0, -10));
        GetComponent<Collider2D>().enabled = false;
        selectionMarker.SetVisible(false);
        _animator.SetTrigger("Die");
        InActiveAnimation = true;
    }

    void SetMarkerState(bool isVisible, SelectionMarker.Type type)
    {
        selectionMarker.SetType(type);
        _uiCharCard.SetType(type);
        selectionMarker.SetVisible(isVisible);
        _uiCharCard.SetSelected(isVisible);
    }

    public void SetShieldEffectVisible(bool isVisible)
    {
        ShieldEffect.SetActive(isVisible);
        _uiCharCard.SetShieldEffectVisible(isVisible);
    }

    public void InvokeAnimCallback() => _animationEventCallback?.Invoke();

    // public void Shoot()
    // {
    //     foreach (CharacterView target in _targetsForDisplaingSkill)
    //     {
    //         Projectile projectile = Instantiate(ProjectilePrefab, _projectileSpawnPos.position, Quaternion.identity);
    //         projectile.Init(target.ProjectileHit.position);
    //     }
    // }

}
