using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharactersCardsManager : MonoBehaviour
{
    [SerializeField] CharactersRegistry registry;
    [SerializeField] UICharacterCard uiCharacterCardPrefab;
    [SerializeField] Transform PlayerTeamContainer;
    [SerializeField] Transform NPCTeamContainer;
    public UICharacterCard CreateCard(Character character)
    {
        UICharacterCard newCard = Instantiate(uiCharacterCardPrefab, character.Team == 0 ? PlayerTeamContainer : NPCTeamContainer);
        newCard.SetAvatar(registry.Get(character._data).UIIcon);
        return newCard;
    }
}
