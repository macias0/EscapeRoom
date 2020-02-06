using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class WhetstoneController : MonoBehaviour, IUsable
{

    private new AudioSource audio;

    [SerializeField]
    InventoryController inventoryController;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public Item Use(Item other = null)
    {

        if(other is Sword)
        {
            Sword sword = other as Sword;
            if (!sword.sharp)
            {
                sword.sharp = true;
                sword.name = "Stalowy miecz";
                sword.description = "Ostry jak brzytwa";
                sword.combinable = false;
                audio.Play();
                //deselect sword
                inventoryController.selectedItem = null;
            }
        }
        return null;
    }
}
