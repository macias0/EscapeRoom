using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : Item
{
    [SerializeField]
    private bool hasBattery = false;

    

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
            if (active)
            {
                var fl = PlayerController.mainPlayer.GetComponentInChildren<Flashlight_PRO>();
                //Debug.Log("Flashligh go: " + fl + "," + fl.gameObject);
                Destroy(fl.gameObject);
            }
            else
            {
                GameObject lightPrefab = Resources.Load("Prefabs/Flashlight") as GameObject;
                Instantiate(lightPrefab, PlayerController.mainPlayer.transform);
                PlayerController.mainPlayer.GetComponentInChildren<Flashlight_PRO>().Switch();
            }
            active = !active;
        }
        else
            Debug.Log("Brakuje bateri");

        return active ? this : null;
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
