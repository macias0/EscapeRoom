using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class DoorController : MonoBehaviour, IUsable
{


    enum EDoorState
    {
        Idle,
        Opening,
        Closing
    }


    [SerializeField]
    private int keyId = 0;

    [SerializeField]
    private bool _locked = true;

    public bool locked { get => _locked; set => _locked = value; }

    [SerializeField]
    private Vector3 startRot;

    [SerializeField]
    private Vector3 endRot;

    [SerializeField]
    private float animSpeed = 0.2f;

    [SerializeField]
    private float threshold = 1.0f;

    private EDoorState state = EDoorState.Idle;


    private bool open = false;

    [SerializeField]
    private AudioClip unlockClip;

    [SerializeField]
    private AudioClip openCloseClip;

    private new AudioSource audio;


    void Start()
    {
        audio = GetComponent<AudioSource>();
    }


    //method alias - used in UnityEvent, 
    //because it doesn't see method with return type other that void
    public void Open()
    {
        state = EDoorState.Opening;
        if (openCloseClip != null)
        {
            audio.clip = openCloseClip;
            audio.Play();
        }
    }

    public void Close()
    {
        state = EDoorState.Closing;
        if (openCloseClip != null)
        {
            audio.clip = openCloseClip;
            audio.Play();
        }
    }


    public Item Use(Item other = null)
    {
        if(other is Key)
        {
            Debug.Log("Mamy klucz");
            if( ((Key)other).keyId == keyId )
            {
                if(unlockClip != null)
                {
                    audio.clip = unlockClip;
                    audio.Play();
                }

                Debug.Log("Odblokowalem dzrwi");
                locked = false;
                return other;
            }
        }

        if (!locked && state == EDoorState.Idle)
        {
            Debug.Log("OTwieram drzwi: open: "+ open);
            if (open) Close();
            else Open();

            //Vector3 rot = transform.localEulerAngles;
            //rot.y = open ? 0 : 90;
            //transform.localEulerAngles = rot;
            //open = !open;

        }

        return null;
    }

    public void Update()
    {
        Vector3 targetRot = Vector3.zero;
        if(state == EDoorState.Opening)
        {
            targetRot = endRot; 
        }
        else if (state == EDoorState.Closing)
        {
            targetRot = startRot;
        }
        if(state != EDoorState.Idle)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * animSpeed);
            if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(targetRot)) <= threshold)
            {
                Debug.Log("Skonczylem z dzrzwiami");
                state = EDoorState.Idle;
                open = !open;
            }
        }
    }
}
