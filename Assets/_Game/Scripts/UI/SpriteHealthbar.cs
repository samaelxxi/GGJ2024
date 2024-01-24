using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHealthbar : MonoBehaviour
{
    [SerializeField] Transform Mask;

    float _percent = 1;

    public void SetValue(float newValue)
    {
        _percent = newValue;
        Mask.transform.localScale = new Vector3( 1, newValue, 1);
    }

}
