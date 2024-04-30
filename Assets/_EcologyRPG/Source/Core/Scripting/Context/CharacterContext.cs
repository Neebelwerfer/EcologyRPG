using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using MoonSharp.Interpreter;
using System;
using UnityEngine;

namespace EcologyRPG.Core.Scripting
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



        public void ApplyCondition(CastContext context, int conditionID)
        {
            var existing = Character.GetConditionReference(conditionID);
            if(existing != null)
            {
                existing.OnReapply();
                return;
            }
            var condition = ConditionReferenceDatabase.Instance.GetCondition(conditionID);
            if(condition != null)
            {
                Character.ApplyCondition(context, condition);
            }
            else
            {
                Debug.LogError("Condition with ID " + conditionID + " not found");
            }
        }

        public bool Compare(CharacterContext other)
        {
            if(other == null)
            {
                return false;
            }
            return Character.GUID == other.Character.GUID;
        }

        public bool IsLegalMove(Vector3Context dir, float speed)
        {
            return BaseCharacter.IsLegalMove(Character, dir.Vector, speed);
        }

        public void ApplyStatModifier(StatModifierContext modifier)
        {
            Character.AddStatModifier(modifier.Modifier);
        }

        public void RemoveStatModifier(StatModifierContext modifier)
        {
            Character.RemoveStatModifier(modifier.Modifier);
        }

        public void ApplyUniqueStatModifier(StatModifierContext modifier, bool minVal)
        {
            Character.AddUniqueStatModifier(modifier.Modifier, minVal);
        }

        public void RemoveUniqueStatModifier(StatModifierContext modifier)
        {
            Character.RemoveUniqueStatModifier(modifier.Modifier);
        }


        public void RotateTowards(Vector3Context target)
        {
            Character.RotateTowards(target.Vector);
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

        public void IgnoreCollision()
        {
            Character.IgnoreCollision();
        }

        public void RestoreCollision()
        {
            Character.RestoreCollision();
        }

        public void RestoreMovement()
        {
            StartMovement();
            StartRotation();
            RemoveSlow();
        }
    }
}