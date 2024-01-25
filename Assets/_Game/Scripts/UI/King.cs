using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{

    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        Game.Instance.Events.OnCharacterDamaged += (_, _) => 
        {
            _animator.SetTrigger("Lol");
        };
    }
}
