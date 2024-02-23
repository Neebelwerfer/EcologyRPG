using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public void Use()
    {
        Debug.Log("Using " + Name);
    }
}
