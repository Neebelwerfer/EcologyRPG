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
    Stat HealthRegen;

    public override void Initialize(PlayerCharacter player)
    {
        Health = player.Stats.GetResource("health");
        HealthRegen = player.Stats.GetStat("healthRegen");
        Stamina = player.Stats.GetResource("stamina");
        StaminaGain = player.Stats.GetStat("staminaGain");
        Water = player.Stats.GetResource("water");
        WaterDrain = player.Stats.GetStat("waterDrain");
        Food = player.Stats.GetResource("food");
        FoodDrain = player.Stats.GetStat("foodDrain");
    }
    public override void Update()
    {
        Stamina += StaminaGain.Value * TimeManager.IngameDeltaTime;
        Water -= WaterDrain.Value * TimeManager.IngameDeltaTime;
        Food -= FoodDrain.Value * TimeManager.IngameDeltaTime;
        if (Water.CurrentValue > 0 && Food.CurrentValue > 0)
        {
            Health += HealthRegen.Value * TimeManager.IngameDeltaTime;
        }

    }

    public override void OnDestroy()
    {

    }
}
