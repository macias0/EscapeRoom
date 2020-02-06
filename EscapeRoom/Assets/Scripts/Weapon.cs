using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item, IFireable
{

    protected GameObject model = null;
    protected string modelPath = "";

    [SerializeField]
    protected float range = 5.0f;

    public virtual RaycastHit? Fire()
    {
        model.GetComponent<Animator>().SetTrigger("Attack");

        AudioSource aSrc = model.GetComponent<AudioSource>();
        if (!aSrc.isPlaying)
            aSrc.Play();


        //check if you hit something
        RaycastHit hit;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(ray, out hit, range))
        {
            return hit;
            
        }
        else
            return null;
    }

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
            if (active)
            {
                //var go = PlayerController.mainPlayer.transform.Find("Crowbar");
                Destroy(model);
            }
            else
            {
                GameObject crowbarPrefab = Resources.Load(modelPath) as GameObject;
                model = Instantiate(crowbarPrefab, PlayerController.mainPlayer.transform);
            }
            active = !active;
        }

        return active ? this : null;

    }

}
