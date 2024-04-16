using EcologyRPG.Utility;
using System;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    public class Item : ScriptableObject, IEquatable<Item>, IEquatable<string>
    {
        public enum ItemTypes
        {
            Item,
            ReplenishingItem,
            Weapon,
            Armor,
            Mask,
            WaterTank
        }

        public enum ItemCategory
        {
            Item,
            Equipment,
            Consumable,
        }

        public const string ItemPath = "Assets/_EcologyRPG/Items";

        [Header("Item Properties")]
        [ReadOnlyString]
        public string GUID;
        public string Name;
        public ItemCategory Category;
        public string Description;
        public Sprite Icon;
        public float Weight;
        [HideInInspector] public ItemTypes ItemType;
        [HideInInspector] public GenerationRules generationRules;

        protected string DisplayString;

        public bool CanGenerate { get { return generationRules != null; } }

        public virtual InventoryItem Generate(int level)
        {
            BasicGenerationRules generationRules = this.generationRules as BasicGenerationRules;
            var newInventoryItem = new InventoryItem(this, UnityEngine.Random.Range(generationRules.minDropAmount, generationRules.maxDropAmount));
            return newInventoryItem;
        }

        public virtual void Initialize()
        {
            
        }

        public bool Equals(string other)
        {
            return GUID == other;
        }

        public virtual bool Equals(Item other)
        {
            return GUID == other.GUID;
        }

        public virtual string GetDisplayString()
        {
            return "Decsription: " + Description + "\n" + "Weight: " + Weight + "\n";
        }
    }
}