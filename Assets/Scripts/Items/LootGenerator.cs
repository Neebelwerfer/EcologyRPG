using Character;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootGenerator
{
    static LootGenerator _instance;
    public static LootGenerator Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LootGenerator();
            }
            return _instance;
        }
    }

    GameObject ItemPickupPrefab;
    LootDatabase lootDatabase;
    BaseCharacter Player;

    private LootGenerator() 
    {
        lootDatabase = Resources.Load<LootDatabase>("Config/Loot Database");
        Player = CharacterManager.Instance.GetCharacterByTag("Player");
        ItemPickupPrefab = Resources.Load<GameObject>("Prefabs/ItemPrefab");
    }

    public void GenerateLootOnKill(BaseCharacter deadNPC)
    {
        var lootChanceRoll = Player.Random.NextFloat(0, 100);
        Debug.Log("Loot Chance Roll: " + lootChanceRoll);
        if (lootChanceRoll > lootDatabase.lootChance)
            return;

        var lootAmount = Player.Random.NextInt(lootDatabase.minLootAmount, lootDatabase.maxLootAmount);

        var loot = lootDatabase.GetItemTemplates(Player.Random, deadNPC.Tags, lootAmount);

        foreach (var item in loot)
        {
            var generatedItem = item.GenerateItem(Player.Random.NextInt(Player.Level - 1, Player.Level + 1));

            var origin = deadNPC.transform.position;
            var point = UnityEngine.Random.insideUnitCircle * 2;
            var position = new Vector3(point.x, 0, point.y);
            var itemPickup = GameObject.Instantiate(ItemPickupPrefab, origin + position, Quaternion.identity);
            var itemPickupComponent = itemPickup.GetComponentInChildren<ItemPickup>();
            itemPickupComponent.Setup(generatedItem.item, generatedItem.amount);

        }
    }
}
