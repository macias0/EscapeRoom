using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInventoryItemStatus
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

    public Item selectedItem { get => _selectedItem; set => _selectedItem = value; }

    public Item _usedItem = null;

    public Item usedItem { get => _usedItem; private set => _usedItem = value; }

    public bool active { get; private set; }

    private InventoryPainter inventoryPainter = null;


    void Start()
    {
        Cursor.visible = false;
        active = false;
        inventoryPainter = inventoryCanvas.GetComponentInChildren<InventoryPainter>();
    }

    //returns true if item should be toggled in the inventory, otherwise false
    public EInventoryItemStatus ItemClicked(Item item)
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
                Debug.Log("RES: " + res);

                if (usedItem && usedItem != item && res != null)
                {
                    
                    usedItem.Use(); //disable current item

                    inventoryPainter.Paint(inventory.inventory, null);
                   

                }

                if (res == item)
                {
                    Debug.Log("ITEM IN USE");
                    usedItem = item;
                    return EInventoryItemStatus.InUse;
                }
                else if (res == null)
                {
                    Debug.Log("ITEM USE NONE");
                    if(item == usedItem)
                        usedItem = null;
                    return EInventoryItemStatus.None;
                }
            }
        }
        else if (selectedItem == item)
        {
            selectedItem = null;
            Debug.Log("Odznaczam");
            return EInventoryItemStatus.None;
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
                inventoryPainter.Paint(inventory.inventory, selectedItem);
            }
            else
            {
                Debug.Log("Nie mozesz polaczyc tych przedmiotow, item active: " + item + item.active);
                return item.active ? EInventoryItemStatus.InUse :  EInventoryItemStatus.None;
               
            }

            selectedItem = null;
            
        }

        return EInventoryItemStatus.Selected;

    }


    public void ToggleInventory()
    {

            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
            active = inventoryCanvas.activeSelf;
            Cursor.visible = inventoryCanvas.activeSelf;

            if (active)
                inventoryPainter.Paint(inventory.inventory, selectedItem);
            else
                inventoryPainter.Clear();

    }

    public void Interaction()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        //Debug.DrawRay(transform.position, transform.forward * pickUpDistance, Color.red);
        if (Physics.Raycast(ray, out hit, pickUpDistance))
        {
            GameObject go = hit.collider.gameObject;
            if (go.tag == "Interactive")
            {
                IPickable pickable = go.GetComponent(typeof(IPickable)) as IPickable;
                if (pickable != null)
                {
                    Debug.Log("Podnosze item");

                    inventory.AddItem(pickable.PickUp());

                    //Destroy(go);
                }
                else
                {
                    
                    IUsable usable = go.GetComponent(typeof(IUsable)) as IUsable;
                    if (usable == null)
                        usable = go.GetComponentInParent(typeof(IUsable)) as IUsable;

                    Debug.Log("Szukam IUSABLe w obiekcie: " + go + "i parencie: " + usable);
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

    public void Fire()
    {
        IFireable weapon = usedItem as IFireable;
        if(weapon != null)
        {
            weapon.Fire();
        }
    }

}
