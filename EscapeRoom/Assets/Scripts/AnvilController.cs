using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AnvilController : MonoBehaviour, IUsable
{
    private new AudioSource audio;

    [SerializeField]
    private GameObject bar;

    [SerializeField]
    private GameObject sword;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public Item Use(Item other = null)
    {
        Debug.Log("USE NA ANVIL, other=" + other);
        if(other is Hammer && bar.activeSelf)
        {
            audio.Play();
            bar.SetActive(false);
            sword.SetActive(true);
            return other;
            
        }
        else if(other is Bar)
        {
            bar.SetActive(true);
            return other;
        }
        return null;
    }

}
