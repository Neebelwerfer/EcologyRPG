using EcologyRPG.Core.Items;
using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Interactables
{
    enum LootType
    {
        Specific,
        GeneratedLoot
    }

    [System.Serializable]
    class ItemGroup
    {
        public ItemReference itemRef;
        public int amount;
    }

    [CreateAssetMenu(menuName = "Interactables/Loot Spawner")]
    public class LootSpawner : Interaction
    {
        [SerializeField] LootType lootType;
        [SerializeField] ItemGroup[] loot = new ItemGroup[0];

        [SerializeField, Min(0)] int minLoot;
        [SerializeField, Min(1)] int maxLoot;
        [SerializeField, CharacterTag] List<string> tags = new();

        public override void Interact()
        {
            if(lootType == LootType.Specific)
            {
                for (int i = 0; i < loot.Length; i++)
                {
                    var item = Game.Items.GetItemByGUID(loot[i].itemRef.GUID);
                    ItemDisplayHandler.Instance.SpawnItem(item, loot[i].amount, LootGenerator.Instance.FindLegalSpawnPoint(Player.PlayerCharacter.Transform.Position, 2));
                }
            }
            else
            {
                var items = LootGenerator.Instance.LootDatabase.GetItemTemplates(Player.PlayerCharacter.Random, tags, Random.Range(minLoot, maxLoot));
                foreach (var item in items)
                {
                    var generatedItem = item.Generate(Player.PlayerCharacter.Random.NextInt(Player.PlayerCharacter.Level - 1, Player.PlayerCharacter.Level + 1));
                    ItemDisplayHandler.Instance.SpawnItem(generatedItem.item, generatedItem.amount, LootGenerator.Instance.FindLegalSpawnPoint(Player.PlayerCharacter.Transform.Position, 2));
                }
            }
        }
    }
}
