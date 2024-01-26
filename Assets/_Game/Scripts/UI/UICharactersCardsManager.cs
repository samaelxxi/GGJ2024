using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] CharactersRegistry registry;
    [SerializeField] UICharacterCard uiCharacterCardPrefab;
    [SerializeField] Transform PlayerTeamContainer;
    [SerializeField] Transform NPCTeamContainer;

    [SerializeField] GameObject WinScreenPrefab;
    [SerializeField] GameObject DefeatScreenPrefab;

    public void DisplayCombatEnd(bool isPeremoga)
    {
        GameObject prefab = isPeremoga ? WinScreenPrefab : DefeatScreenPrefab;
        Instantiate(prefab, transform);
    }

    public UICharacterCard CreateCard(Character character)
    {
        UICharacterCard newCard = Instantiate(uiCharacterCardPrefab, character.Team == 0 ? PlayerTeamContainer : NPCTeamContainer);
        newCard.SetAvatar(registry.Get(character._data).UIIcon);
        return newCard;
    }
}
