using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using NaughtyAttributes.Test;
using Unity.VisualScripting;
using UnityEngine;

public class UIView : MonoBehaviour
{
    [SerializeField] CharacterView CharacterViewPrefab;
    [SerializeField] List<Transform> PlayerTeamSlots;
    [SerializeField] List<Transform> NPCTeamSlots;

    [SerializeField] SkillsPanel SkillsPanel;

    List<CharacterView> PlayerCharactersViews = new List<CharacterView>(3);
    List<CharacterView> NPCCharactersViews = new List<CharacterView>(3);

    Combat _combat;
    State _state = State.ChooseTarget;

    Character _targetCharacter;
    CharacterView _activeCharacterView;
    SkillBtn _selectedSkillBtn;

    public void SelectSkillBtn(SkillBtn newSkillBtn)
    {
        if (_selectedSkillBtn)
        {
            _selectedSkillBtn.IsSelected = false;
        }
        _selectedSkillBtn = newSkillBtn;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(Combat combat)
    {
        _combat = combat;
        // Create characters
        int i = 0;
        foreach (Character character in _combat._team1)
        {
            CharacterView newCharacterView = Instantiate(CharacterViewPrefab, PlayerTeamSlots[i]);
            newCharacterView.Init(character);
            PlayerCharactersViews.Add(newCharacterView);
            i++;
        }
        i = 0;
        foreach (Character character in _combat._team2)
        {
            CharacterView newCharacterView = Instantiate(CharacterViewPrefab, NPCTeamSlots[i]);
            //newCharacterView.transform.localScale = new Vector3(1,-1,1);
            newCharacterView.Init(character);
            NPCCharactersViews.Add(newCharacterView);
            i++;
        }
        Game.Instance.Events.OnCharacterGetsTurn += OnCharactersTurn;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.DisplayAction:

                break;
            case State.ChooseSkill:
                ChooseSkillUpdate();
                break;
            case State.ChooseTarget:
                ChooseTargetUpdate();
                break;
        }
    }

    void OnCharactersTurn(Character character)
    {
        if (_activeCharacterView = PlayerCharactersViews.FirstOrDefault((CharacterView cv) => cv.Character == character))
        {
            Debug.Log("Player team turn");
            _activeCharacterView.SetSelectedState(true);
            SkillsPanel.ShowSkills(_activeCharacterView.Character._data.Skills);
            _state = State.ChooseSkill;
        }
        else if (_activeCharacterView = NPCCharactersViews.FirstOrDefault((CharacterView cv) => cv.Character == character))
        {
            Debug.Log("NPC team turn");
            SkillsPanel.Hide();
        }
    }

    void DisplayActionUpdate()
    {
        _state = State.WaitForEnemyTurnEnd;
    }

    void ChooseSkillUpdate()
    {
        if (_selectedSkillBtn) // Skill will be set byt skill btn
        {
            Debug.Log($"{_selectedSkillBtn.name} active");
            _state = State.ChooseTarget;
        }
    }

    void ChooseTargetUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SelectSkillBtn(null);
            _state = State.ChooseSkill;
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;

            CharacterView charecterView = hitObject.GetComponentInParent<CharacterView>();
            if (!charecterView) return;
            
            charecterView.Highlight();
            if (Input.GetMouseButtonDown(0))
            {
                _targetCharacter = charecterView.Character;
                if(_selectedSkillBtn.Skill.IsAOE) _targetCharacter = null;
                charecterView.SetSelectedState(true);
                _combat.UseSkill(_activeCharacterView.Character, _selectedSkillBtn.Skill, _targetCharacter);
                SkillsPanel.Hide();
                _state = State.DisplayAction;
                // execute skill
            }

            // apply chosen skill

        }

    }
    void SetCursorInTargetState() { }

    private enum State
    {
        ChooseSkill,
        ChooseTarget,
        WaitForEnemyTurnEnd,
        DisplayAction
    }
}
