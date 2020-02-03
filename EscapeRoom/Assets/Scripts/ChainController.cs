using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainController : MonoBehaviour, IUsable
{
    public Item Use(Item other = null)
    {
        if(other.GetType() == typeof(Chemical) && other.name == "Kwas siarkowy VI")
        {
            gameObject.SetActive(false);
            return other;
        }
        return null;
    }

}
