using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ItemController : MonoBehaviour, IPickable
{

    
    enum EItemType {
        None,
        Note,
        Battery,
        Flashlight,
        Key,
        Crowbar
    };

    [SerializeField]
    private EItemType itemType = EItemType.None;

    Dictionary<EItemType, Type> itemTypeToTypeDictionary = new Dictionary<EItemType, Type> {

        { EItemType.Note, typeof(Note) },
        { EItemType.Battery, typeof(Battery)},
        { EItemType.Flashlight, typeof(Flashlight)},
        { EItemType.Key, typeof(Key)},
        { EItemType.Crowbar, typeof(Crowbar)}
    };

    [SerializeField]
    private Item _item = null;

    public Item item { get => _item; private set => _item = value; }

    private void OnValidate()
    {
        //item = (Item)Activator.CreateInstance(dict[itemType]);
        if (itemType != EItemType.None && (item == null || (item.GetType() != itemTypeToTypeDictionary[itemType])))
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
