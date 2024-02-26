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

[CreateAssetMenu(fileName = "Healing Potion", menuName = "Items/Consumable/HealingPotion")]
public class HealingPotion : ConsumableItem
{
    public override void Use(BaseCharacter player)
    {
        base.Use(player);
        player.stats.GetResource("Health").ModifyCurrentValue(20);
    }
}