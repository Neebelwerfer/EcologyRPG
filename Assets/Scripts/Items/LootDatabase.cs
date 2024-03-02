using Items.ItemTemplates;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Loot
{
    public ItemTemplate ItemTemplate;
    [CharacterTag]
    public List<string> allowedTags = new List<string>();
}

[System.Serializable]
public class CategoryOdds
{
    [ReadOnlyString]
    public string category;
    public int odds;
    public List<Loot> items;

    public CategoryOdds(string category, int odds)
    {
        this.category = category;
        this.items = new();
        this.odds = odds;
    }
}

[CreateAssetMenu(fileName = "New Loot Database", menuName = "Items/Loot Database")]
public class LootDatabase : ScriptableObject
{
    [SerializeField] List<Loot> items = new List<Loot>();


    [Header("Loot Rules")]
    public float lootChance = 0.5f;
    public int minLootAmount = 1;
    public int maxLootAmount = 6;

    public List<CategoryOdds> CategoryOdds = new List<CategoryOdds>();

    public LootDatabase()
    {
        CategoryOdds.Add(new CategoryOdds("Basic Items", 75));
        CategoryOdds.Add(new CategoryOdds("Equipable Items", 25));
    }

    private void OnValidate()
    {
        var basicItems = CategoryOdds[0];
        var EquipableItems = CategoryOdds[1];
        basicItems.items.Clear();
        EquipableItems.items.Clear();

        foreach (Loot loot in items)
        {
            if (loot.ItemTemplate is BasicItem)
            {
                CategoryOdds[0].items.Add(loot);
            }
            else if (loot.ItemTemplate is EquipableItemTemplate)
            {
                EquipableItems.items.Add(loot);
            }
        }
    }

    public List<Loot> GetRandomCategory(Unity.Mathematics.Random random, List<string> tags)
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
            totalOdds += odds.odds;
        }

        var roll = random.NextInt(0, totalOdds);
        int CulmulativeOdds = 0;
        for (int i = 0; i < AllowedLists.Count; i++)
        {
            if(roll < AllowedLists[i].odds + CulmulativeOdds)
            {
                return AllowedLists[i].items;
            }
            CulmulativeOdds += AllowedLists[i].odds;
        }
        return null;
    }   
}

#if UNITY_EDITOR

public class ReadOnlyString : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyString))]
public class CategoryOddsDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(position, label.text, property.stringValue);
    }
}
#endif