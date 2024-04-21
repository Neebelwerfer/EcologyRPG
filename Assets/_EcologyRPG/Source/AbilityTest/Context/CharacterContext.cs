using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;

namespace EcologyRPG.AbilityScripting
{
    public class CharacterContext
    {
        readonly BaseCharacter Character;

        public CharacterContext(BaseCharacter character)
        {
            Character = character;
        }

        public void TriggerAnimation(string animationName)
        {
            Character.Animator.SetTrigger(animationName);
        }

        public void ApplyDamage(float damage, int damageType)
        {
            var damageInfo = new DamageInfo()
            {
                damage = damage,
                type = (DamageType)damageType
            };
            Character.ApplyDamage(damageInfo);
        }

        public Vector3Context GetCastPos()
        {
            return new Vector3Context(Character.CastPos);
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
            return Character.Stats.GetResource(resourceName);
        }

        public Stat GetStat(string statName)
        {
            return Character.Stats.GetStat(statName);
        }

        public void SlowMovement()
        {
            Character.SlowMovementSpeed();
        }

        public void RemoveSlow()
        {
            Character.RestoreMovementSpeed();
        }

        public void StopRotation()
        {
            Character.StopRotation();
        }

        public void StartRotation()
        {
            Character.StartRotation();
        }

        public void StopMovement()
        {
            Character.StopMovement();
        }

        public void StartMovement()
        {
            Character.StartMovement();
        }

        public void RestoreMovement()
        {
            StartMovement();
            StartRotation();
            RemoveSlow();
        }

    }
}