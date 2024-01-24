using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    
    [SerializeField] SpriteRenderer SelectedMarker;
    [SerializeField] SpriteHealthbar Healthbar;
    [SerializeField] Transform _body;
    public Character Character {get; set;}

    bool _isHighlighted = false;
    // Start is called before the first frame update
    void UpdateStatus(){
        Healthbar.SetValue(Character._data.TotalHealth);
    }
    void Start()
    {
        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
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
        SelectedMarker.enabled = true;
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
}
