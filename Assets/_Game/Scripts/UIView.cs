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

    LinkedList<VisualEvent> _visualEvents;

    Combat _combat;
    State _state = State.ChooseTarget;

    bool _unprocesedEventsInQUeue = false;

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
        CreateCharacters();
        SubscribeToEvents();
        _state = State.DisplayAction;
    }

    void CreateCharacters()
    {
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
    }
    void SubscribeToEvents()
    {
        _visualEvents = new LinkedList<VisualEvent>();
        Game.Instance.Events.OnCharacterGetsTurn += (Character c) =>
                {
                    _visualEvents.AddLast(new CharacterGetsTurnVE(c));
                    _unprocesedEventsInQUeue = true;
                };
        Game.Instance.Events.OnCharacterDamaged += (Character c, int damage) =>
        {
                    _visualEvents.AddLast(new CharacterDamagedVisualEvent(c, damage));
                    _unprocesedEventsInQUeue = true;
        };
        
    }
    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case State.DisplayAction:
                DisplayActionUpdate();
                break;
            case State.ChooseSkill:
                ChooseSkillUpdate();
                break;
            case State.ChooseTarget:
                ChooseTargetUpdate();
                break;
            // case State.WaitForEnemyTurnEnd:
            //     _state = State.ChooseSkill;
            //     break;
            default:
                Debug.LogError("Invalid state");
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
    IEnumerator DisplayActions()
    {
        foreach (VisualEvent ve in _visualEvents)
        {
            yield return StartCoroutine(ve.Display());
            if (ve is CharacterGetsTurnVE charTurnVE && charTurnVE.CharacterView.Character.Team == 0)
            {
                _activeCharacterView = charTurnVE.CharacterView;
                _state = State.ChooseSkill;
                SkillsPanel.ShowSkills(_activeCharacterView.Character._data.Skills);
            }
        }
        _visualEvents.Clear();
    }
    void DisplayActionUpdate()
    {
        if (_unprocesedEventsInQUeue)
        {
            StartCoroutine(DisplayActions());
            _unprocesedEventsInQUeue = false;
        }
        //_state = State.WaitForEnemyTurnEnd;
    }

    void ChooseSkillUpdate()
    {
        if (_selectedSkillBtn) // Skill will be set byt skill btn
        {
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
            _targetCharacter = charecterView.Character;
            if (!_combat.IsSkillUsageCorrect(_activeCharacterView.Character, _selectedSkillBtn.Skill, _targetCharacter))
            {
                _targetCharacter = null;
                if (Input.GetMouseButtonDown(0)) { /* play nope sound  */} 
                return;
            }
            charecterView.Highlight();
            if (_selectedSkillBtn.Skill.IsAOE)
            {
                List<CharacterView> WholeTeam = _targetCharacter.Team == 0 ? PlayerCharactersViews : NPCCharactersViews;
                foreach (var character in WholeTeam)
                {
                    character.Highlight();
                }
            }


            if (Input.GetMouseButtonDown(0))
            {
                //if(_selectedSkillBtn.Skill.IsAOE) _targetCharacter = null;
                _combat.UseSkill(_activeCharacterView.Character, _selectedSkillBtn.Skill, _targetCharacter);
                SkillsPanel.Hide();
                _activeCharacterView.SetSelectedState(false);
                _selectedSkillBtn = null;
                _state = State.DisplayAction;
            }
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

    public CharacterView GetViewByCharacter(Character character)
    {
        List<CharacterView> Team = character.Team == 0 ? PlayerCharactersViews : NPCCharactersViews;
        return Team.FirstOrDefault((CharacterView cv) => cv.Character == character);
    }
}
