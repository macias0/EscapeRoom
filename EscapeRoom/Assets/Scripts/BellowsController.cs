using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class BellowsController : MonoBehaviour, IUsable
{
    private new AudioSource audio;

    [SerializeField]
    private FurnanceController furnance;

    [SerializeField]
    private GameObject fireplace;


    void Start()
    {
        audio = GetComponent<AudioSource>();
    }


    public Item Use(Item other = null)
    {
        audio.Play();
        fireplace.SetActive(true);
        furnance.hot = true;
        return null;
    }
}



