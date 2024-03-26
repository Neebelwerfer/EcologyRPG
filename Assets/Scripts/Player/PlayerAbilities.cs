using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class PlayerAbility
{
    public PlayerAbilityDefinition ability;
    public uint LevelRequirement;
}

[CreateAssetMenu(fileName = "Ability Database", menuName = "Player/Player Ability Database")]
public class PlayerAbilities : ScriptableObject
{
    [SerializeField] List<PlayerAbility> abilities;

    public int Count => abilities.Count;

    public List<PlayerAbilityDefinition> GetAllPlayerAbilities()
    {
        return abilities.ConvertAll(x => x.ability);
    }

    public List<PlayerAbilityDefinition> GetPlayerAbilities(uint level)
    {
        return abilities.FindAll(x => x.LevelRequirement <= level).ConvertAll(x => x.ability);
    }
}