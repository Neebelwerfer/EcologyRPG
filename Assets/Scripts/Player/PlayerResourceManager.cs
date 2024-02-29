using Character;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class PlayerResourceManager : PlayerModule
{
    Resource Health;
    Resource Stamina;
    Resource Water;
    Resource Food;

    Stat StaminaGain;
    Stat WaterDrain;
    Stat FoodDrain;

    public override void Initialize(PlayerCharacter player)
    {
        Health = player.stats.GetResource("health");
        Stamina = player.stats.GetResource("stamina");
        StaminaGain = player.stats.GetStat("staminaGain");
        Water = player.stats.GetResource("water");
        WaterDrain = player.stats.GetStat("waterDrain");
        Food = player.stats.GetResource("food");
        FoodDrain = player.stats.GetStat("foodDrain");
    }
    public override void Update()
    {
        Stamina += StaminaGain.Value * TimeManager.IngameDeltaTime;
        Water -= WaterDrain.Value * TimeManager.IngameDeltaTime;
        Food -= FoodDrain.Value * TimeManager.IngameDeltaTime;

    }

    public override void OnDestroy()
    {

    }
}
