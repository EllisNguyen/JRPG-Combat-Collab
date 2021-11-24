using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class ConsumableObject : ItemObject
{
    private void Awake()
    {
        itemType = ItemType.Health;
    }
}
