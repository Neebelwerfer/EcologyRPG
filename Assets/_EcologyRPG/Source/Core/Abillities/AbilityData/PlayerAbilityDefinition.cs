using Codice.Client.Commands;
using EcologyRPG.Core.Character;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace EcologyRPG.Core.Abilities.AbilityData
{
    [CreateAssetMenu(fileName = "new Player ability data", menuName = "Ability/Player Ability Data")]
    public class PlayerAbilityDefinition : AttackAbilityDefinition
    {
        [Header("Resources")]
        [Tooltip("The resource that get used for the ability cost"), StatAttribute(StatType.Resource)]
        public string ResourceName;
        [Tooltip("The resource cost of this ability")]
        public float ResourceCost = 0;
        [Tooltip("The description of the ability"), TextArea(1, 5)]
        public string Description;
        [Tooltip("The trigger to set in the animator when the ability is casted")]
        public string AnimationTrigger;

        public bool BlockMovementOnWindup = false;
        public bool ReducedSpeedOnWindup = true;
        public bool BlockRotationOnWindup = true;
        public bool UseMouseDirection = false;
        public bool RotatePlayerTowardsMouse = false;
        static readonly StatModification HalfSpeed = new StatModification("movementSpeed", -0.75f, StatModType.PercentMult, null);

        int triggerHash;
        Resource resource;
        Vector3 MousePoint;

        public override void Initialize(BaseCharacter owner)
        {
            base.Initialize(owner);
            triggerHash = Animator.StringToHash(AnimationTrigger);
            if (ResourceName != "")
            {
                resource = owner.Stats.GetResource(ResourceName);
            }
        }

        public override bool CanActivate(BaseCharacter caster)
        {
            if (!base.CanActivate(caster)) return false;
            if (ResourceName != "" && caster.Stats.GetResource(ResourceName) < ResourceCost)
            {
                return false;
            }
            return true;
        }

        public override IEnumerator HandleCast(CastInfo caster)
        {
            if (ResourceCost > 0)
            {
                InitialCastCost(caster);
            }
            return base.HandleCast(caster);
        }

        public override void CastStarted(CastInfo castInfo)
        {
            if (triggerHash != 0)
            {
                castInfo.owner.Animator.SetTrigger(triggerHash);
            }
            if (BlockRotationOnWindup)
            {
                Debug.Log("Stop Rotation");
                castInfo.owner.StopRotation();
            }

            if (BlockMovementOnWindup) castInfo.owner.StopMovement();
            if (ReducedSpeedOnWindup) castInfo.owner.Stats.AddStatModifier(HalfSpeed);
            base.CastStarted(castInfo);

            var res = TargetUtility.GetMousePoint(Camera.main);
            MousePoint = res;

            if (RotatePlayerTowardsMouse)
            {
                res.y = castInfo.owner.Transform.Position.y;
                Debug.DrawRay(castInfo.owner.Transform.Position, (res - castInfo.owner.Transform.Position).normalized, Color.red, 1f);
                castInfo.owner.Rigidbody.MoveRotation(Quaternion.LookRotation(res - castInfo.owner.Transform.Position));
            }
        }

        public override void CastCancelled(CastInfo caster)
        {
            base.CastCancelled(caster);
            resource += ResourceCost;
            if (BlockMovementOnWindup) caster.owner.StartMovement();
            if (BlockRotationOnWindup) caster.owner.StartRotation();
            if (ReducedSpeedOnWindup) caster.owner.Stats.RemoveStatModifier(HalfSpeed);
        }


        public override void CastFinished(CastInfo castInfo)
        {
            if (UseMouseDirection)
            {
                castInfo.dir = (MousePoint - castInfo.owner.Transform.Position).normalized;
                Debug.DrawRay(castInfo.owner.Transform.Position, castInfo.dir, Color.blue, 1f);
            }
            castInfo.targetPoint = MousePoint;
            base.CastFinished(castInfo);
            if (BlockMovementOnWindup) castInfo.owner.StartMovement();
            if (BlockRotationOnWindup)
            {
                Debug.Log("Start Rotation");
                castInfo.owner.StartRotation();
            }

            if (ReducedSpeedOnWindup) castInfo.owner.Stats.RemoveStatModifier(HalfSpeed);
        }

        /// <summary>
        /// Called when the cast is started to deduct the resource cost
        /// </summary>
        /// <param name="caster"></param>
        protected virtual void InitialCastCost(CastInfo caster)
        {
            resource -= ResourceCost;
        }
    }
}