using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;

namespace EcologyRPG.AbilityTest
{
    public class CharacterContext
    {
        BaseCharacter Character;
        public CharacterContext(BaseCharacter character)
        {
            Character = character;
        }

        public Resource GetResource(string resourceName)
        {
            return (Character.Stats.GetResource(resourceName));
        }

        public StatContext GetStat(string statName)
        {
            return new StatContext(Character.Stats.GetStat(statName));
        }

    }
}