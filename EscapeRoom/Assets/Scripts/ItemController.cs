using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ItemController : MonoBehaviour, IPickable
{

    
    enum ItemType {
        None,
        Note,
        Battery,
        Flashlight,
        Key,
        Crowbar
    };

    [SerializeField]
    private ItemType itemType = ItemType.None;

    Dictionary<ItemType, Type> itemTypeToTypeDictionary = new Dictionary<ItemType, Type> {

        { ItemType.Note, typeof(Note) },
        { ItemType.Battery, typeof(Battery)},
        { ItemType.Flashlight, typeof(Flashlight)},
        { ItemType.Key, typeof(Key)},
        { ItemType.Crowbar, typeof(Crowbar)}
    };

    [SerializeField]
    private Item _item = null;

    public Item item { get => _item; private set => _item = value; }

    private void OnValidate()
    {
        //item = (Item)Activator.CreateInstance(dict[itemType]);
        if (itemType != ItemType.None && (item == null || (item.GetType() != itemTypeToTypeDictionary[itemType])))
        {
            Debug.Log("Tworze nowy item");
            item = (Item)ScriptableObject.CreateInstance(itemTypeToTypeDictionary[itemType]);
        }
    }

    public Item PickUp()
    {
        Destroy(gameObject);
        return item;
    }
}
