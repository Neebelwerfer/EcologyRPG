using EcologyRPG.Core.Items;
using System;
using UnityEngine;

namespace EcologyRPG.GameSystems.Crafting
{
    [Serializable, CreateAssetMenu(fileName = "Item Recipe", menuName = "Recipe/Item Recipe")]
    public class ItemRecipe : Recipe
    {
        public Item CraftedItem;

        public override void Craft()
        {

            base.Craft();
            Player.PlayerInventory.AddItem(CraftedItem);
        }
    }
}