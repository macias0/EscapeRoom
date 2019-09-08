using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : Weapon
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
                GameObject crowbarPrefab = Resources.Load("Prefabs/Crowbar") as GameObject;
                model = Instantiate(crowbarPrefab, PlayerController.mainPlayer.transform);
            }
            active = !active;
        }

        return active ? this : null;

    }


    public override void Fire()
    {
        Debug.Log("Jeb z łomika");
        model.GetComponent<Animator>().SetTrigger("Attack");

        AudioSource aSrc = model.GetComponent<AudioSource>();
        if(!aSrc.isPlaying)
            aSrc.Play();


        //check if you hit something
        RaycastHit hit;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if(Physics.Raycast(ray, out hit, range ))
        {
            GameObject go = hit.collider.gameObject;
            if(go && go.tag == "Interactive" )
            {
                IBreakable target = go.GetComponent(typeof(IBreakable)) as IBreakable;
                if(target != null)
                {
                    target.Hit(this, hit);
                }
            }
        }
    }



}
