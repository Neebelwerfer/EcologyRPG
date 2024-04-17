using EcologyRPG.Core.Character;
using System.Collections;
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

        public float ToxicResoureCost = 5;
        public float ToxicSelfDamage = 10;
        public BaseAbility ToxicAbility;

        public bool BlockMovementOnWindup = false;
        public bool ReducedSpeedOnWindup = true;
        public bool BlockRotationOnWindup = true;
        public bool UseMouseDirection = false;
        public bool RotatePlayerTowardsMouse = false;
        static readonly StatModification HalfSpeed = new StatModification("movementSpeed", -0.75f, StatModType.PercentMult, null);

        public int TriggerHash { get; protected set; }
        Resource resource;
        Resource toxicResource;
        Vector3 MousePoint;

        public override void Initialize(BaseCharacter owner, AbilityDefintion prefabAbility)
        {
            base.Initialize(owner, prefabAbility);
            TriggerHash = Animator.StringToHash(AnimationTrigger);
            if (ResourceName != "")
            {
                resource = owner.Stats.GetResource(ResourceName);
            }
            if(ToxicAbility != null)
            {
                toxicResource = owner.Stats.GetResource(AbilityManager.ToxicResourceName);
            }
        }

        string GetResourceDisplayName()
        {
            if(AbilityManager.UseToxic && ToxicAbility != null)
            {
                var toxicResourceName = Stats.StatsData.Resources.Find(x => x.name == AbilityManager.ToxicResourceName).DisplayName;
                return $"{toxicResourceName}: {ToxicResoureCost}";
            }
            else if (ResourceName != "")
            {
                var res = Stats.StatsData.Resources.Find(x => x.name == ResourceName).DisplayName;
                return $"{res}: {ResourceCost}";
            }
            return "";
        }

        public virtual string GetDescription()
        {
            return $"Cooldown: {Cooldown}" +
                $"\n{GetResourceDisplayName()}" +
                $"\n{Description}";
        }

        public override bool CanActivate(BaseCharacter caster)
        {
            if (!base.CanActivate(caster)) return false;
            if (ToxicAbility == null || !AbilityManager.UseToxic)
            {
                if (ResourceName != "" && resource < ResourceCost)
                {
                    return false;
                }
            }
            else
            {
                if(toxicResource < ToxicResoureCost)
                {
                    return false;
                }
            }
            return true;
        }

        public override IEnumerator HandleCast(CastInfo caster)
        {

            InitialCastCost(caster);
            return base.HandleCast(caster);
        }

        public override void CastStarted(CastInfo castInfo)
        {
            if (TriggerHash != 0)
            {
                castInfo.owner.Animator.SetTrigger(TriggerHash);
            }
            if (BlockRotationOnWindup) castInfo.owner.StopRotation();
            if (BlockMovementOnWindup) castInfo.owner.StopMovement();
            if (ReducedSpeedOnWindup) castInfo.owner.Stats.AddStatModifier(HalfSpeed);

            var res = TargetUtility.GetMousePoint(Camera.main);
            MousePoint = res;
            if (UseMouseDirection)
            {
                var mouse = MousePoint;
                mouse.y = castInfo.owner.Transform.Position.y;
                castInfo.dir = (mouse - castInfo.owner.Transform.Position).normalized;
                Debug.DrawRay(castInfo.owner.Transform.Position, castInfo.dir, Color.blue, 1f);
            }
            base.CastStarted(castInfo);



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
                var mouse = MousePoint;
                mouse.y = castInfo.owner.Transform.Position.y;
                castInfo.dir = (mouse - castInfo.owner.Transform.Position).normalized;
                Debug.DrawRay(castInfo.owner.Transform.Position, castInfo.dir, Color.blue, 1f);
            }
            castInfo.targetPoint = MousePoint;

            if (ToxicAbility != null && AbilityManager.UseToxic)
            {
                ToxicAbility.Cast(castInfo);
            }
            else
            {
                base.CastFinished(castInfo);
            }

            if (BlockMovementOnWindup) castInfo.owner.StartMovement();
            if (BlockRotationOnWindup) castInfo.owner.StartRotation();

            if (ReducedSpeedOnWindup) castInfo.owner.Stats.RemoveStatModifier(HalfSpeed);
        }

        /// <summary>
        /// Called when the cast is started to deduct the resource cost
        /// </summary>
        /// <param name="caster"></param>
        public virtual void InitialCastCost(CastInfo caster)
        {
            if(ToxicAbility != null && AbilityManager.UseToxic)
            {
                toxicResource -= ToxicResoureCost;
                caster.owner.ApplyDamage(new DamageInfo() { damage = ToxicSelfDamage, source = caster.owner, type = DamageType.Toxic });
            }
            else if (ResourceName != "")
            {
                resource -= ResourceCost;
            }
        }
    }
}