using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery :  Item
{
    public float level = 100.0f;

    public void Awake()
    {
        combinable = true;
    }
    
}
