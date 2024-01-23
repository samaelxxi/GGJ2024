using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NaughtyAttributes.Test;
using UnityEngine;

public class UIView : MonoBehaviour
{

    List<CharacterView> Characters;
    Combat _combat;
    State _state = State.ChooseTarget;

    Character _targetCharacter;
    public Skill Skill {get; set;}

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(Combat combat)
    {
        _combat = combat;
        // Create characters
        _combat.
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(_state)
        {
            case State.ChooseTarget:
                ChooseTargetUpdate();
            break;
        }
    }

    void OnCharactersTurn(Character character)
    {

    }

    void ChooseTargetUpdate(){
         if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;
                CharacterView charecterView = hitObject.GetComponent<CharacterView>();
                if(!charecterView) return;

                _targetCharacter = charecterView.Character;
                charecterView.SetSelectedState(true);
                // apply chosen skill
                
            }
        }
    }

    void ShowSkillsMenu(){}
    void SetCursorInTargetState(){}
    
             private enum State
    {
        ChooseSkill,
        ChooseTarget,
        WaitForEnemyTurnEnd
    }
}
