using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPickable
{
    Item PickUp();

}

interface IUsable
{
    Item Use(Item other = null);
}
