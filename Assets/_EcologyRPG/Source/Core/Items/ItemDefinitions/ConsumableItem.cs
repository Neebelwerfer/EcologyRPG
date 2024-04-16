using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    public class ConsumableItem : Item
    {
        public ConsumableItem()
        {
            Category = ItemCategory.Consumable;
        }

        public virtual void Use(BaseCharacter player)
        {
            Debug.Log("Using " + Name);
        }
    }
}