using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPickable
{
    Item PickUp();

}

interface IUsable
{
    //if returned value is null - failed to use, if its "this" then success, if other then other item has been used and should be destroyed 
    Item Use(Item other = null);
}
