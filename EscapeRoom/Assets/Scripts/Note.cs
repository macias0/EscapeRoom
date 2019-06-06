using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Note : Item
{
    [SerializeField]
    private string _content = "";
    public string content { get => _content;  }



    public void Awake()
    {
        sprite = Resources.Load<Sprite>("Graphics/Inventory/Note");
    }


}
