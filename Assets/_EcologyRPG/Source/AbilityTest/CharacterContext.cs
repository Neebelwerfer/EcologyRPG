using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;

namespace EcologyRPG.AbilityTest
{
    public class CharacterContext
    {
        readonly BaseCharacter Character;
        public CharacterContext(BaseCharacter character)
        {
            Character = character;
        }

        public Vector3Context GetPosition()
        {
            return new Vector3Context(Character.Transform.Position);
        }

        public void SetPosition(Vector3Context position)
        {
            Character.Transform.Position = position.Vector;
        }

        public Vector3Context GetVelocity()
        {
            return new Vector3Context(Character.Velocity);
        }
        public void SetVelocity(Vector3Context velocity)
        {
            Character.Velocity = velocity.Vector;
        }

        public Vector3Context GetForward()
        {
            return new Vector3Context(Character.Transform.Forward);
        }

        public Vector3Context GetRight()
        {
            return new Vector3Context(Character.Transform.Right);
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