using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IUsable
{

    [SerializeField]
    private int keyId = 0;

    [SerializeField]
    private bool locked = true;

    private bool open = false;

    public Item Use(Item other = null)
    {
        if(other is Key)
        {
            Debug.Log("Mamy klucz");
            if( ((Key)other).keyId == keyId )
            {
                Debug.Log("Odblokowalem dzrwi");
                locked = false;
                return other;
            }
        }

        if (!locked)
        {

            Vector3 rot = transform.localEulerAngles;
            rot.y = open ? 0 : 90;
            transform.localEulerAngles = rot;
            open = !open;
        }

        return null;
    }
}
