using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterCard : MonoBehaviour
{
    [SerializeField] Image avatar;
    [SerializeField] Image healtbar;
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
}
