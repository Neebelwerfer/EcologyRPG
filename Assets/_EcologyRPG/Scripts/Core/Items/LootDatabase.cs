using Items.ItemTemplates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

[System.Serializable]
public class Loot
{
    public ItemTemplate ItemTemplate;
    public int weight = 0;
    [CharacterTag]
    public List<string> allowedTags = new List<string>();
}

[System.Serializable]
public class CategoryOdds
{
    public string category;
    public int weight;
    public List<Loot> items;

    public CategoryOdds(string category, int weight)
    {
        this.category = category;
        this.items = new();
        this.weight = weight;
    }
}

[CreateAssetMenu(fileName = "New Loot Database", menuName = "Items/Loot Database")]
public class LootDatabase : ScriptableObject
{
    [Header("Loot Rules")]
    public float lootChance = 0.5f;
    public int minLootAmount = 1;
    public int maxLootAmount = 6;

    public List<CategoryOdds> CategoryOdds = new List<CategoryOdds>();

    public List<ItemTemplate> GetItemTemplates(Unity.Mathematics.Random random, List<string> tags, int amount)
    {
        List<CategoryOdds> AllowedLists = new List<CategoryOdds>();

        for (int i = 0; i < CategoryOdds.Count; i++)
        {
            if (CategoryOdds[i].items.Count > 0 && CategoryOdds[i].items.Any((x) => x.allowedTags.Count == 0 || x.allowedTags.Any((y) => tags.Contains(y))))
            {
                AllowedLists.Add(CategoryOdds[i]);
            }
        }

        int totalOdds = 0;
        foreach (var odds in AllowedLists)
        {
            totalOdds += odds.weight;
        }

        List<ItemTemplate> loot = new();

        for (int i = 0; i < amount; i++)
        {
            var roll = random.NextInt(0, totalOdds);
            int CulmulativeOdds = 0;
            for (int j = 0; j < AllowedLists.Count; j++)
            {
                if (roll < AllowedLists[j].weight + CulmulativeOdds)
                {
                    var list = AllowedLists[j].items.Where(loot => loot.allowedTags.Count == 0 || loot.allowedTags.Any((tag) => tags.Contains(tag))).ToArray();
                    loot.Add(GetItemTemplate(list));
                }
                CulmulativeOdds += AllowedLists[j].weight;
            }
        }
        return loot;
    }

    public ItemTemplate GetItemTemplate(Loot[] lootArray)
    {
        int maximumWeight = 0;
        foreach (var loot in lootArray)
        {
            maximumWeight += loot.weight;
        }

        int roll = Random.Range(0, maximumWeight);
        int culmulativeWeight = 0;
        for (int i = 0; i < lootArray.Length; i++)
        {
            if (roll < lootArray[i].weight + culmulativeWeight)
            {
                return lootArray[i].ItemTemplate;
            }
            culmulativeWeight += lootArray[i].weight;
        }
        return null;
    }
}