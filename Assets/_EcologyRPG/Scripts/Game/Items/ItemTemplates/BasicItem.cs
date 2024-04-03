using EcologyRPG.Core.Items;
using UnityEngine;

namespace EcologyRPG.GameSystems.Items
{
    [CreateAssetMenu(fileName = "New Item template", menuName = "Items/Templates/Item")]
    public class BasicItem : ItemTemplate
    {
        [Header("Item Template Properties")]
        public int minDropAmount;
        public int maxDropAmount;

        public override InventoryItem GenerateItem(int level)
        {
            var item = CreateInstance<Item>();
            item.Name = Name;
            item.Description = Description;
            item.Icon = Icon;
            item.Weight = Weight;

            return new InventoryItem(item, Random.Range(minDropAmount, maxDropAmount));
        }
    }
}