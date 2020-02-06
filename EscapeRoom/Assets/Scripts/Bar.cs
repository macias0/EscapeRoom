using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : Item
{
    [SerializeField]
    private bool _hot = false;

    public bool hot { get => _hot; set => _hot = value; }
}
