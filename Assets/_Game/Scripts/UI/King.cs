using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class King : MonoBehaviour
{

    Animator _animator;
    // Start is called before the first frame update

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        // _animator = GetComponent<Animator>();
        // Game.Instance.Events.OnCharacterDamaged += (_, _) => 
        // {
        //     _animator.SetTrigger("Lol");
        // };
    }

    public void Lol(float delay = 0){
        StartCoroutine(ActualLol(delay));
    }

    IEnumerator ActualLol(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetTrigger("Lol");
    }
}
