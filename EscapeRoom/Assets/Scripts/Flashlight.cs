using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item
{
    [SerializeField]
    private bool hasBattery = false;

    private bool enabled = false;

    public Item Combine(Item other)
    {
        if (other.GetType() == typeof(Battery))
        {
            hasBattery = true;
            description = "Latarka ma baterie";
            return this;
        }
        else
            return null;
    }

    public override Item Use(Item other)
    {

        if (other != null)
            return Combine(other);


        if (hasBattery)
        {
            Debug.Log("Latarka swieci");
            if (enabled)
                Destroy(PlayerController.mainPlayer.GetComponentInChildren<Light>().gameObject);
            else
            {
                GameObject lightPrefab = Resources.Load("Prefabs/FlashlightPrefab") as GameObject;
                Instantiate(lightPrefab, PlayerController.mainPlayer.transform);
            }
            enabled = !enabled;
        }
        else
            Debug.Log("Brakuje bateri");

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
