using EcologyRPG.GameSystems.Interactables;
using EcologyRPG.Core.Items;
using EcologyRPG.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.Interactables
{
    enum LootType
    {
        Specific,
        GeneratedLoot
    }

    [CreateAssetMenu(menuName = "Interactables/Loot Spawner")]
    public class LootSpawner : Interaction
    {
        [SerializeField] LootType lootType;
        [SerializeField] InventoryItem[] loot = new InventoryItem[0];

        [SerializeField, Min(0)] int minLoot;
        [SerializeField, Min(1)] int maxLoot;
        [SerializeField, CharacterTag] List<string> tags = new();

        public override void Interact()
        {
            if(lootType == LootType.Specific)
            {
                for (int i = 0; i < loot.Length; i++)
                {
                    var item = Instantiate(loot[i].item);
                    ItemDisplayHandler.Instance.SpawnItem(item, loot[i].amount, LootGenerator.Instance.FindLegalSpawnPoint(Player.PlayerCharacter.Transform.Position, 2));
                }
            }
            else
            {
                var items = LootGenerator.Instance.LootDatabase.GetItemTemplates(Player.PlayerCharacter.Random, tags, Random.Range(minLoot, maxLoot));
                foreach (var item in items)
                {
                    var generatedItem = item.GenerateItem(Player.PlayerCharacter.Random.NextInt(Player.PlayerCharacter.Level - 1, Player.PlayerCharacter.Level + 1));
                    ItemDisplayHandler.Instance.SpawnItem(generatedItem.item, generatedItem.amount, LootGenerator.Instance.FindLegalSpawnPoint(Player.PlayerCharacter.Transform.Position, 2));
                }
            }
        }
    }
}

[CustomEditor(typeof(LootSpawner))]
public class LootSpawnerDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        var lootType = serializedObject.FindProperty("lootType");
        var loot = serializedObject.FindProperty("loot");
        var tags = serializedObject.FindProperty("tags");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("OneTimeUse"));
        EditorGUILayout.PropertyField(lootType);

        if(lootType.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(loot);
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minLoot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxLoot"));
            EditorGUILayout.PropertyField(tags);
        }

        serializedObject.ApplyModifiedProperties();
    }
}