using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    [SerializeField]
    private int _keyId;

    public int keyId { get => _keyId; private set => _keyId = value; }

    public void Awake()
    {
        combinable = true;
    }
}
