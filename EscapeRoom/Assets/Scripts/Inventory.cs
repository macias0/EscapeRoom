using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{

    public List<Item> _inventory = new List<Item>();

    //copy, not reference
    public List<Item> inventory { get => _inventory; }

    public void AddItem(Item it)
    {
        _inventory.Add(it);
    }
    public bool RemoveItem(Item it)
    {
        return _inventory.Remove(it);
    }

}
