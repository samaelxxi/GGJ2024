using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterCard : MonoBehaviour
{
    [SerializeField] Image avatar;
    [SerializeField] Image healtbar;

    [SerializeField] Image Indicator;

    [SerializeField] Color AllyColor;
    [SerializeField] Color EnemyColor;
    [SerializeField] Color SelectedColor = Color.white;

    [SerializeField] GameObject ShieldStatusEffect;
    float _percent = 1;

    public void SetHealthValue(float newValue)
    {
        _percent = newValue;
        healtbar.fillAmount = _percent;
    }

    public void SetAvatar(Sprite sprite)
    {
        avatar.sprite = sprite;
    }

    public void SetType(SelectionMarker.Type type)
    {
        switch (type)
        {
            case SelectionMarker.Type.Selection:
                Indicator.color = SelectedColor;
                break;
            case SelectionMarker.Type.Enemy:
                Indicator.color = EnemyColor;
                break;
            case SelectionMarker.Type.Ally:
                Indicator.color = AllyColor;
                break;
        }
    }

    public void SetShieldEffectVisible(bool isVisible) => ShieldStatusEffect.SetActive(isVisible);

    public void SetSelected(bool isSelected)
    {
        Indicator.enabled = isSelected;
    }
}
