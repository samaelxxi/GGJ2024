using Unity.VisualScripting;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] SpriteHealthbar Healthbar;
    [SerializeField] Transform _body;
    public Character Character { get; set; }
    
    Animator _animator;

    public bool InActiveAnimation = false;

    bool _isHighlighted = false;
    // Start is called before the first frame update
    public void UpdateStatus()
    {
        Healthbar.SetValue((float)Character.Health / Character._data.TotalHealth);
    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
        if (Character.IsDead) return;
        if (_isHighlighted)
        {
            _isHighlighted = false;
            _body.localScale = new Vector3(1.1f, 1.1f, 1f);
        }
        else
        {
            _body.localScale = Vector3.one;
        }
    }


    public void Init(Character character)
    {
        Character = character;
        // set sprite and other stuff
    }

    public void Highlight()
    {
        _isHighlighted = true;
    }

    public void DisplayAttack()
    {
        _animator.SetTrigger("Attack");
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
