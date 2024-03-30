using EcologyRPG.Core.Abilities.AbilityData;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Game.Player
{
    [System.Serializable]
    public class PlayerAbilityData
    {
        public PlayerAbilityDefinition ability;
        public uint LevelRequirement = 1;
    }

    [CreateAssetMenu(fileName = "Ability Database", menuName = "Player/Player Ability Database")]
    public class PlayerAbilities : ScriptableObject
    {
        [SerializeField] List<PlayerAbilityData> abilities = new();

        public int Count => abilities.Count;

        public List<PlayerAbilityDefinition> GetAllPlayerAbilities()
        {
            return abilities.ConvertAll(x => x.ability);
        }

        public List<PlayerAbilityDefinition> GetPlayerAbilities(uint level)
        {
            return abilities.FindAll(x => x.LevelRequirement <= level).ConvertAll(x => x.ability);
        }

        public void AddAbility(PlayerAbilityDefinition ability, uint level)
        {
            abilities.Add(new PlayerAbilityData() { ability = ability, LevelRequirement = level });
        }

        public List<PlayerAbilityData> GetList() => abilities;
    }
}