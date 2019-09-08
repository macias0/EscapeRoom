using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(AudioSource))]
public class DoorController : MonoBehaviour, IUsable
{
    private new AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    enum EDoorState
    {
        Idle,
        Opening,
        Closing
    }


    [SerializeField]
    private int keyId = 0;

    [SerializeField]
    private bool locked = true;

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

    public Item Use(Item other = null)
    {
        if(other is Key)
        {
            Debug.Log("Mamy klucz");
            if( ((Key)other).keyId == keyId )
            {
                audio.Play();
                Debug.Log("Odblokowalem dzrwi");
                locked = false;
                return other;
            }
        }

        if (!locked && state == EDoorState.Idle)
        {
            Debug.Log("OTwieram drzwi: open: "+ open);
            if (open) state = EDoorState.Closing;
            else state = EDoorState.Opening;
                
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
