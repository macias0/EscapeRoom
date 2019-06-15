using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryItemStatus
{
    None,
    Selected,
    InUse
};


public class InventoryController : MonoBehaviour
{



    [SerializeField]
    private float pickUpDistance = 5.0f;
    private Inventory _inventory = new Inventory();
    public Inventory inventory { get => _inventory; }

    [SerializeField]
    private GameObject inventoryCanvas = null;

    private Item _selectedItem = null;

    public Item selectedItem { get => _selectedItem; private set => _selectedItem = value; }

    public bool active { get; private set; }


    void Start()
    {
        Cursor.visible = false;
        active = false;
    }

    //returns true if item should be toggled in the inventory, otherwise false
    public InventoryItemStatus ItemClicked(Item item)
    {

        if (selectedItem == null )
        {
            if (item.combinable)
            {
                Debug.Log("Zaznaczam item " + item);
                selectedItem = item;
            }
            else
            {
                Item res = item.Use();
                if (res == item)
                    return InventoryItemStatus.InUse;
                else if (res == null)
                    return InventoryItemStatus.None;
            }
        }
        else if (selectedItem == item)
        {
            selectedItem = null;
            Debug.Log("Odznaczam");
            return InventoryItemStatus.None;
        }
        else
        {
            Debug.Log("Craftuje itemy: " + item.name + " z " + selectedItem.name);
            Item craftedItem = item.Use(selectedItem);

            if (craftedItem)
            {
                inventory.RemoveItem(item);
                inventory.RemoveItem(selectedItem);
                inventory.AddItem(craftedItem);
                inventoryCanvas.GetComponentInChildren<InventoryPainter>().Paint(inventory.inventory, selectedItem);
            }
            else
            {
                Debug.Log("Nie mozesz polaczyc tych przedmiotow");
                return InventoryItemStatus.None;
               
            }

            selectedItem = null;
            
        }

        return InventoryItemStatus.Selected;

    }


    public void ToggleInventory()
    {

            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
            active = inventoryCanvas.activeSelf;
            Cursor.visible = inventoryCanvas.activeSelf;

            if (active)
                inventoryCanvas.GetComponentInChildren<InventoryPainter>().Paint(inventory.inventory, selectedItem);
            else
                inventoryCanvas.GetComponentInChildren<InventoryPainter>().Clear();

    }

    public void Interaction()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * pickUpDistance, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickUpDistance))
        {
            GameObject go = hit.collider.gameObject;
            if (go.tag == "Interactive")
            {
                IPickable pickable = (IPickable)go.GetComponent(typeof(IPickable));
                if (pickable != null)
                {
                    Debug.Log("Podnosze item");

                    inventory.AddItem(pickable.PickUp());

                    Destroy(go);
                }
                else
                {
                    IUsable usable = (IUsable)go.GetComponent(typeof(IUsable));
                    if (usable != null)
                    {
                        Item ret = usable.Use(selectedItem);
                        // if returned item is the same as current item, it means that item is already used and should be deleted
                        if (ret == selectedItem)
                        {
                            inventory.RemoveItem(selectedItem);
                            selectedItem = null;
                        }
                    }
                }


            }
        }
    }

}
