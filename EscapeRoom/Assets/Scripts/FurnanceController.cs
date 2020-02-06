using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnanceController : MonoBehaviour, IUsable
{

    [SerializeField]
    private GameObject bar;

    [SerializeField]
    private InventoryController inventoryController;

    [SerializeField]
    private bool _hot = false;

    public bool hot { get => _hot; set => _hot = value; }

    public Item Use(Item other = null)
    {
           
        if(hot)
        {
            if(other is Bar && !bar.activeSelf)
            {
                bar.SetActive(true);
                return other;

            }
            if (other is Pilers && bar.activeSelf)
            {
                IPickable pickable = bar.GetComponent(typeof(IPickable)) as IPickable;
                inventoryController.inventory.AddItem(pickable.PickUp());
                return other;
            }
            

        }
        return null;
    }
}

