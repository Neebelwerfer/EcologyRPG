using EcologyRPG.Core.Character;
using EcologyRPG.Utility;

namespace EcologyRPG.GameSystems.PlayerSystems
{
    public class PlayerResourceManager
    {
        Resource Health;
        Resource Stamina;
        Resource Water;
        Resource Food;

        readonly Stat StaminaGain;
        readonly Stat WaterDrain;
        readonly Stat FoodDrain;
        readonly Stat HealthRegen;

        public PlayerResourceManager(PlayerCharacter player)
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

        public void Update()
        {
            Stamina += StaminaGain.Value * TimeManager.IngameDeltaTime;
            Water -= WaterDrain.Value * TimeManager.IngameDeltaTime;
            Food -= FoodDrain.Value * TimeManager.IngameDeltaTime;
            if (Water.CurrentValue > 0 && Food.CurrentValue > 0)
            {
                Health += HealthRegen.Value * TimeManager.IngameDeltaTime;
            }

        }
    }
}