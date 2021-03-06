﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : Weapon
{


    Crowbar()
    {
        modelPath = "Prefabs/Crowbar";
    }


    public override RaycastHit? Fire()
    {

        RaycastHit? rh = base.Fire();
        if (rh != null)
        {
            RaycastHit hit = rh.Value;
            GameObject go = hit.collider.gameObject;
            if (go && go.tag == "Interactive")
            {
                IHitable target = go.GetComponent(typeof(IHitable)) as IHitable;
                if (target != null)
                {
                    target.Hit(this, hit);
                }
            }
            
        }
        return rh;


    }



}
