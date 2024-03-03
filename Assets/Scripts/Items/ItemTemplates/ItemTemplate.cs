using Character;
using System.Collections.Generic;
using UnityEngine;

namespace Items.ItemTemplates
{
    public enum GrowthType
    {
        Flat,
        Percentage
    }

    [System.Serializable]
    public class StatRanges
    {
        [Header("Stat Modifiers")]
        public string statName;
        public StatModType modType;
        public float minValue;
        public float maxValue;
        [Header("Growth Per Level")]
        public float GrowthPerLevel;
        public GrowthType growthType;
    }

    public abstract class ItemTemplate : ScriptableObject
    {
        [Header("Item Properties")]
        public string Name;
        public string Description;
        public Sprite Icon;
        public float Weight;

        public abstract InventoryItem GenerateItem(int level);
    }
}