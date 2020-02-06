using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class ChainController : MonoBehaviour, IUsable
{
    private new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public Item Use(Item other = null)
    {
        if(other.GetType() == typeof(Chemical) && other.name == "Kwas siarkowy VI")
        {
            float clipLength = audio.clip.length;
            audio.Play();
            StartCoroutine(DestroyChain(clipLength));
            return other;
        }
        return null;
    }

    private IEnumerator DestroyChain(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        gameObject.SetActive(false);

    }

}
