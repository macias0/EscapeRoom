using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : Item, IWeapon
{

    private GameObject model = null;


    public Item Combine(Item other)
    {
        return null;
    }



    public override Item Use(Item other)
    {
        if (other)
            return null;
        else
        {
            if(active)
            {
                //var go = PlayerController.mainPlayer.transform.Find("Crowbar");
                Destroy(model);
            }
            else
            {
                GameObject lightPrefab = Resources.Load("Prefabs/Crowbar") as GameObject;
                model = Instantiate(lightPrefab, PlayerController.mainPlayer.transform);
            }
            active = !active;
        }

        return active ? this : null;

    }


    public void Fire()
    {
        Debug.Log("Jeb z łomika");
        //throw new System.NotImplementedException();
    }



}
