using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField]
    bool _sharp = false;

    public bool sharp { get => _sharp; set => _sharp = value; }

    Sword()
    {
        modelPath = "Prefabs/Sword";
    }


    public override Item Use(Item other)
    {
        if (!sharp)
            return null;
        else
            return base.Use(other);
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
