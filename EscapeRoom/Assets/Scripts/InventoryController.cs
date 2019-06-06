using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called before the first frame update
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

    public bool ItemClicked(Item item)
    {

        if (selectedItem == null )
        {
            Debug.Log("Zaznaczam item: " + item);
            selectedItem = item;
            item.Use();
        }
        else if (selectedItem == item)
        {
            selectedItem.Use();
            selectedItem = null;
            Debug.Log("Odznaczam");
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
                return false;
               
            }

            selectedItem = null;
            
        }

        return true;

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
