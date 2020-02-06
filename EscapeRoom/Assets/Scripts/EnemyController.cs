using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class EnemyController : MonoBehaviour, IHitable
{

    private new AudioSource audio;

    [SerializeField]
    private AudioClip laughClip;

    [SerializeField]
    private AudioClip deathClip;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void Hit(Item weapon, RaycastHit hit)
    {
        if(weapon is Sword)
        {
            audio.clip = deathClip;
            float clipLength = audio.clip.length;
            StartCoroutine(DestroyEnemy(clipLength));
        }
        else
        {
            audio.clip = laughClip;
        }
        audio.Play();
    }

    private IEnumerator DestroyEnemy(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        gameObject.SetActive(false);

    }
}
