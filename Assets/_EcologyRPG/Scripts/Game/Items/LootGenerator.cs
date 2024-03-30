using EcologyRPG.Core.Character;
using EcologyRPG.Game.Player;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    public class LootGenerator
    {
        static LootGenerator _instance;
        public static LootGenerator Instance
        {
            get
            {
                _instance ??= new LootGenerator();
                return _instance;
            }
        }

        LootDatabase lootDatabase;
        BaseCharacter Player;

        private LootGenerator()
        {
            lootDatabase = Resources.Load<LootDatabase>("Config/Loot Database");
            Player = PlayerManager.Instance.GetPlayerCharacter();
        }

        public void GenerateLootOnKill(BaseCharacter deadNPC)
        {
            var lootChanceRoll = Player.Random.NextFloat(0, 100);
            if (lootChanceRoll > lootDatabase.lootChance)
                return;

            var lootAmount = Player.Random.NextInt(lootDatabase.minLootAmount, lootDatabase.maxLootAmount);

            var loot = lootDatabase.GetItemTemplates(Player.Random, deadNPC.Tags, lootAmount);

            foreach (var item in loot)
            {
                var generatedItem = item.GenerateItem(Player.Random.NextInt(Player.Level - 1, Player.Level + 1));

                var origin = deadNPC.Transform.Position;
                var point = UnityEngine.Random.insideUnitCircle * 2;
                var position = new Vector3(point.x, 0, point.y);
                ItemDisplayHandler.Instance.SpawnItem(generatedItem.item, generatedItem.amount, origin + position);
            }
        }
    }
}
