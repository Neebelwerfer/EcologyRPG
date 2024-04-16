using EcologyRPG.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EcologyRPG.Core.Items
{

    [System.Serializable]
    public class CategoryOdds
    {
        public string category;
        public int weight;
        [HideInInspector] public List<Item> items;

        public CategoryOdds(string category, int weight)
        {
            this.category = category;
            this.items = new();
            this.weight = weight;
        }
    }

    public class LootCategory : PropertyAttribute
    {

    }

    [CreateAssetMenu(fileName = "New Loot Database", menuName = "Items/Loot Database")]
    public class LootDatabase : ScriptableObject
    {
        public const string Path = "Assets/_EcologyRPG/Resources/Config/Loot Database.asset";
        public const string ResourcesPath = "Config/Loot Database";

        [Header("Loot Rules")]
        public float lootChance = 0.5f;
        public int minLootAmount = 1;
        public int maxLootAmount = 6;

        public static LootDatabase Load()
        {
            var database = Resources.Load<LootDatabase>(ResourcesPath);
            if (database == null)
            {
                Debug.LogError("No Loot Database found!");
            }
            return database;
        }

        public void Init(ItemDatabase itemDatabase)
        {
            var items = itemDatabase.GetItemsWithGenerationRules();
            int counter = 0;
            foreach (var category in CategoryOdds)
            {
                category.items = items.Where((x) => x.generationRules.DropCategory == category.category).ToList();
                counter += category.items.Count;
            }
            Debug.Log($"Loot Database Initialized: {counter} items found");
        }

        public List<CategoryOdds> CategoryOdds = new List<CategoryOdds>();

        public List<Item> GetItemTemplates(Unity.Mathematics.Random random, List<string> tags, int amount)
        {
            List<CategoryOdds> AllowedLists = new List<CategoryOdds>();

            for (int i = 0; i < CategoryOdds.Count; i++)
            {
                if (CategoryOdds[i].items.Count > 0 && CategoryOdds[i].items.Any((x) => x.generationRules.allowedTags.Length == 0 || x.generationRules.allowedTags.Any((y) => tags.Contains(y))))
                {
                    AllowedLists.Add(CategoryOdds[i]);
                }
            }

            int totalOdds = 0;
            foreach (var odds in AllowedLists)
            {
                totalOdds += odds.weight;
            }

            List<Item> loot = new();

            for (int i = 0; i < amount; i++)
            {
                var roll = random.NextInt(0, totalOdds);
                int CulmulativeOdds = 0;
                for (int j = 0; j < AllowedLists.Count; j++)
                {
                    if (roll < AllowedLists[j].weight + CulmulativeOdds)
                    {
                        var list = AllowedLists[j].items.Where(item => item.generationRules.allowedTags.Length == 0 || item.generationRules.allowedTags.Any((tag) => tags.Contains(tag))).ToArray();
                        loot.Add(GetItemTemplate(list));
                    }
                    CulmulativeOdds += AllowedLists[j].weight;
                }
            }
            return loot;
        }

        public Item GetItemTemplate(Item[] lootArray)
        {
            int maximumWeight = 0;
            foreach (var loot in lootArray)
            {
                maximumWeight += loot.generationRules.DropChanceWeight;
            }

            int roll = Random.Range(0, maximumWeight);
            int culmulativeWeight = 0;
            for (int i = 0; i < lootArray.Length; i++)
            {
                if (roll < lootArray[i].generationRules.DropChanceWeight + culmulativeWeight)
                {
                    return lootArray[i];
                }
                culmulativeWeight += lootArray[i].generationRules.DropChanceWeight;
            }
            return null;
        }
    }
}