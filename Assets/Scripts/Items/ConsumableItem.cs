using Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public virtual void Use(BaseCharacter player)
    {
        Debug.Log("Using " + Name);
    }
}

[CreateAssetMenu(fileName = "replenishing Potion", menuName = "Items/Consumable/replenishingPotion")]
public class ReplenishingPotion : ConsumableItem
{
    public string ResourceName;
    public float AmountToReplenish;

    public override void Use(BaseCharacter player)
    {
        base.Use(player);
        var resource = player.Stats.GetResource(ResourceName);
        resource.ModifyCurrentValue(resource.MaxValue * AmountToReplenish);
    }
}