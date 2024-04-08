using EcologyRPG.Core.Character;
using EcologyRPG.GameSystems;
using EcologyRPG.GameSystems.PlayerSystems;
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
            Player = GameSystems.Player.PlayerCharacter;
        }

        public Vector3 FindLegalSpawnPoint(Vector3 origin, float radius)
        {
            var attempts = 0;   
            while (attempts < 5)
            {
                var point = Random.insideUnitCircle * radius;
                var pos = origin + new Vector3(point.x, 0, point.y);
                pos.y += 100;
                Debug.DrawRay(pos, Vector3.down * 100, Color.red, 5);
                if (Physics.Raycast(pos, Vector3.down, out var hit, 1000, Game.Settings.lootGroundLayer))
                {
                    var hitPoint = hit.point;
                    hitPoint.y += 3;
                    return hitPoint;
                }
                attempts++;
            }
            Debug.LogError("Failed to find a legal spawn point for loot");
            return origin;
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
                ItemDisplayHandler.Instance.SpawnItem(generatedItem.item, generatedItem.amount, FindLegalSpawnPoint(origin, 2));
            }
        }
    }
}
