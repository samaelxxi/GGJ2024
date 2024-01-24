using UnityEngine;

public class CharacterView : MonoBehaviour
{
    
    [SerializeField] SpriteRenderer SelectedMarker;
    [SerializeField] SpriteHealthbar Healthbar;
    [SerializeField] Transform _body;
    public Character Character {get; set;}

    bool _isHighlighted = false;
    // Start is called before the first frame update
    public void UpdateStatus(){
        Healthbar.SetValue((float) Character.Health / Character._data.TotalHealth);
    }
    void Start() { UpdateStatus(); }

    // Update is called once per frame
    void Update()
    {
        if(Character.IsDead) return;
        if(_isHighlighted)
        {
            _isHighlighted = false;
            _body.localScale = new Vector3(1.1f, 1.1f, 1f);
        } else 
        {
            _body.localScale = Vector3.one;
        }
    }

    public void SetSelectedState(bool isSelected){
        //SelectedMarker.enabled = isSelected;
        SelectedMarker.color = isSelected ? Color.red : Color.white;
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

    internal void Die()
    {
        _body.localScale = new Vector3(1.3f, 0.3f, 1);
        Debug.Log("Visualize dead");
    }
}
