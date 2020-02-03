using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chemical : Item
{

    public Item Combine(Item other)
    {
        if (other.GetType() == typeof(Chemical))
        {
            name = "Kwas siarkowy VI";
            description = "Silnie żrący kwas";
            return this;
        }
        return null;
    }

    public override Item Use(Item other)
    {

        if (other != null)
            return Combine(other);
        else
            return base.Use(other);


    }

}
