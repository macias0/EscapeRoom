using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    [SerializeField]
    private int _keyId;

    public int keyId { get => _keyId; private set => _keyId = value; }

    //public override Item Use(Item other = null)
    //{
        
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
