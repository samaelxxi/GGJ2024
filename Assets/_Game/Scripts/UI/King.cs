using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class King : MonoBehaviour
{
    [SerializeField] AudioSource Ambient;
    [SerializeField] AudioSource Music;
    [SerializeField] AudioSource Lols;

    [SerializeField] List<AudioClip> LolSounds;

    Animator _animator;
    // Start is called before the first frame update

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SpawnKing()
    {
        _animator.SetTrigger("CombatStart");
        Music.Play();
        Ambient.Play();
    }

    public void StopMusic()
    {
        Music.Stop();
        Ambient.Stop();
    }

    public void Lol(float delay = 0)
    {
        StartCoroutine(ActualLol(delay));
    }

    IEnumerator ActualLol(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetTrigger("Lol");
        Lols.clip = LolSounds[Random.Range(0, LolSounds.Count)];
        Lols.Play();
    }
}
