using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIView : MonoBehaviour
{
    [SerializeField] CharactersRegistry CharactersRegistry;
    [SerializeField] List<Transform> PlayerTeamSlots;
    [SerializeField] List<Transform> NPCTeamSlots;
    [SerializeField] ActivCharacterMarker ActiveCaracterMarker;

    [SerializeField] SkillsPanel SkillsPanel;

    List<CharacterView> PlayerCharactersViews = new List<CharacterView>(3);
    List<CharacterView> NPCCharactersViews = new List<CharacterView>(3);

    Queue<VisualEvent> _visualEvents;

    Combat _combat;
    State _state = State.ChooseTarget;

    bool _unprocesedEventsInQUeue = false;
    bool _isProcessingEvents = false;
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
            CharacterView newCharacterView = Instantiate(CharactersRegistry.Get(character._data), PlayerTeamSlots[i]).GetComponent<CharacterView>();
            newCharacterView.Init(character);
            PlayerCharactersViews.Add(newCharacterView);
            i++;
        }
        i = 0;
        foreach (Character character in _combat._team2)
        {
            CharacterView newCharacterView = Instantiate(CharactersRegistry.Get(character._data), NPCTeamSlots[i]).GetComponent<CharacterView>();
            //newCharacterView.transform.localScale = new Vector3(1,-1,1);
            newCharacterView.Init(character);
            NPCCharactersViews.Add(newCharacterView);
            i++;
        }
    }
    void SubscribeToEvents()
    {
        _visualEvents = new Queue<VisualEvent>();
        Game.Instance.Events.OnCharacterGetsTurn += (Character c) =>
                {
                    _visualEvents.Enqueue(new CharacterGetsTurnVE(c));
                    _unprocesedEventsInQUeue = true;
                };
        Game.Instance.Events.OnCharacterDamaged += (Character c, int damage) =>
        {
            _visualEvents.Enqueue(new CharacterDamagedVisualEvent(c, damage));
            _unprocesedEventsInQUeue = true;
        };
        Game.Instance.Events.OnCharacterHealed += (Character c, int heal) =>
        {
            _visualEvents.Enqueue(new CharacterHealedVE(c, heal));
            _unprocesedEventsInQUeue = true;
        };
        Game.Instance.Events.OnCharacterDied += (Character c) =>
       {
           _visualEvents.Enqueue(new CharacterDeathVE(c));
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


    IEnumerator DisplayActions()
    {
        while (_visualEvents.Count > 0)
        {
            VisualEvent visualEvent = _visualEvents.Dequeue();
            yield return StartCoroutine(visualEvent.Display());
            if (visualEvent is CharacterGetsTurnVE charTurnVE)
            {
                _activeCharacterView = charTurnVE.CharacterView;
                ActiveCaracterMarker.AttachTo(_activeCharacterView);
                if (charTurnVE.CharacterView.Character.Team == 0)
                {
                    _state = State.ChooseSkill;
                    SkillsPanel.ShowSkills(_activeCharacterView.Character._data.Skills);
                }
                else
                {
                    _combat.MakeNextAITurn();
                }

            }

        }
        _isProcessingEvents = false;
    }
    void DisplayActionUpdate()
    {
        if (_unprocesedEventsInQUeue && !_isProcessingEvents)
        {
            _isProcessingEvents = true;
            _unprocesedEventsInQUeue = false;
            StartCoroutine(DisplayActions());
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
                ActiveCaracterMarker.AttachTo(_activeCharacterView);
                _selectedSkillBtn = null;
                _state = State.DisplayAction;
            }
        }

    }
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
