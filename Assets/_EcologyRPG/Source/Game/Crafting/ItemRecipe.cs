using EcologyRPG.Core.Items;
using System;
using UnityEngine;

namespace EcologyRPG.GameSystems.Crafting
{
    [Serializable, CreateAssetMenu(fileName = "Item Recipe", menuName = "Recipe/Item Recipe")]
    public class ItemRecipe : Recipe
    {
        [ItemSelection()]
        public string craftedItemGUID;

        public override void Craft()
        {

            base.Craft();
            var CraftedItem = Game.Items.GetItemByGUID(craftedItemGUID);
            Player.PlayerInventory.AddItem(CraftedItem);
        }
    }
}