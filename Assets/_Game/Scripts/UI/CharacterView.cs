using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    
    [SerializeField] SpriteRenderer SelectedMarker;
    public Character Character {get; set;}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelectedState(bool isSelected){
        SelectedMarker.enabled = true;
    }
    
}
