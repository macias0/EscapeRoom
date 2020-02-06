using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{

    Hammer()
    {
        modelPath = "Prefabs/Hammer";
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
                IUsable target = go.GetComponent(typeof(IUsable)) as IUsable;
                if (target != null)
                {
                    target.Use(this);
                }
            }

        }
        return rh;


    }
}

