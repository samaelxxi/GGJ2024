using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    public CharactersRegistry CharactersRegistry;
    public SkillsViewRegistry SkillsViewRegistry;

    [SerializeField] List<Transform> PlayerTeamSlots;
    [SerializeField] List<Transform> NPCTeamSlots;

    [SerializeField] SkillsPanel SkillsPanel;
    public UIManager UiManager;

    [SerializeField] King _king;
    public Image DramaticShade;

    List<CharacterView> PlayerCharactersViews = new List<CharacterView>(3);
    List<CharacterView> NPCCharactersViews = new List<CharacterView>(3);

    Queue<VisualEvent> _visualEvents;
    VisualEvent _lastInQueue;

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

    public void Init(Combat combat)
    {
        _combat = combat;
        _state = State.DisplayAction;
        SkillsViewRegistry.Preload();
        CharactersRegistry.Preload();
    }

    public void StartDisplaingCombat()
    {
        CreateCharacters();
        SubscribeToEvents();
        Game.Instance.StartCombat();
        UiManager.HideStartMenu();
    }

    void CreateCharacters()
    {
        int i = 0;
        foreach (Character character in _combat._team1)
        {
            CharacterView newCharacterView = Instantiate(CharactersRegistry.Get(character._data).prefab, PlayerTeamSlots[i]).GetComponent<CharacterView>();
            UICharacterCard card = UiManager.CreateCard(character);

            newCharacterView.Init(character, card);
            PlayerCharactersViews.Add(newCharacterView);
            i++;
        }
        i = 0;
        foreach (Character character in _combat._team2)
        {
            CharacterView newCharacterView = Instantiate(CharactersRegistry.Get(character._data).prefab, NPCTeamSlots[i]).GetComponent<CharacterView>();
            UICharacterCard card = UiManager.CreateCard(character);
            newCharacterView.Init(character, card);
            NPCCharactersViews.Add(newCharacterView);
            i++;
        }
    }
    void SubscribeToEvents()
    {
        _visualEvents = new Queue<VisualEvent>();
        Game.Instance.Events.OnCharacterGetsTurn += (Character c) => AddVisualEvent(new CharacterGetsTurnVE(c));

        Game.Instance.Events.OnCharacterDamaged += (Character c, int damage) => AddVisualEvent(new CharacterDamagedVisualEvent(c, damage));

        Game.Instance.Events.OnCharacterHealed += (Character c, int heal) => AddVisualEvent(new CharacterHealedVE(c, heal));

        Game.Instance.Events.OnCharacterDied += (Character c) => AddVisualEvent(new CharacterDeathVE(c));

        Game.Instance.Events.OnSkillUsed += (Character user, Skill skill, List<Character> targets) => AddVisualEvent(new CharacterUsesSkillVE(user, skill, targets));

        Game.Instance.Events.OnCombatEnd += (int teamId) => AddVisualEvent(new CombatEndVE(teamId));

        Game.Instance.Events.OnCharactersGetsEffect += (Character c, Effect e) => AddVisualEvent(new CharactersGetsEffectVE(c, e));

        Game.Instance.Events.OnCharacterEffectEnd += (Character c, Effect e) => AddVisualEvent(new CharacterEffectEndVE(c, e));

    }

    void AddVisualEvent(VisualEvent newVisualEvent)
    {
        if (newVisualEvent is CharacterUsesSkillVE skillEvent)
        {
            skillEvent.CourutineOvner = this;
        }
        if (_lastInQueue != null && _lastInQueue is CharacterUsesSkillVE parentSkillEvent && parentSkillEvent.AttachVisualEvent(newVisualEvent))
        {
            // event is attached to the last in queue
            return;
        }
        _lastInQueue = newVisualEvent;
        _visualEvents.Enqueue(newVisualEvent);
        _unprocesedEventsInQUeue = true;

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
            if (visualEvent is CharacterUsesSkillVE skillEvent && skillEvent.IsAttack)
            {
                _king.Lol(.5f);
            }

            if(visualEvent is CombatEndVE) _king.StopMusic();
            yield return StartCoroutine(visualEvent.Display());

            if (visualEvent is CharacterGetsTurnVE charTurnVE)
            {
                if (_activeCharacterView) _activeCharacterView.IsSelected = false;
                _activeCharacterView = charTurnVE.CharacterView;
                _activeCharacterView.IsSelected = true;
                if (charTurnVE.CharacterView.Character.Team == 0)
                {
                    _state = State.ChooseSkill;
                    SkillsPanel.ShowSkills(_activeCharacterView.Character._data.Skills);
                }
                else
                {
                    yield return new WaitForSeconds(0.5f);
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
                _activeCharacterView.IsSelected = false;
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
