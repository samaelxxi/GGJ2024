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

    public void SpawnKing()
    {
        _animator.SetTrigger("CombatStart");
    }

    public void Lol(float delay = 0)
    {
        StartCoroutine(ActualLol(delay));
    }

    IEnumerator ActualLol(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetTrigger("Lol");
    }
}
