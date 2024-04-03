using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Abilities.AbilityData
{
    public class AttackAbilityDefinition : AbilityDefintion
    {
        public bool BlockMovementOnWindup = false;
        public bool RotatePlayerTowardsMouse = true;
        public bool ReducedSpeedOnWindup = true;
        public bool BlockRotationOnWindup = true;
        public BaseAbility Ability;

        Vector3 MousePoint;
        static StatModification HalfSpeed = new StatModification("movementSpeed", -0.75f, StatModType.PercentMult, null);

        public override void CastStarted(CastInfo caster)
        {
            base.CastStarted(caster);
            if (BlockRotationOnWindup) caster.owner.StopRotation();
            if (BlockMovementOnWindup) caster.owner.StopMovement();
            if (ReducedSpeedOnWindup) caster.owner.Stats.AddStatModifier(HalfSpeed);
            var res = TargetUtility.GetMousePoint(Camera.main);
            MousePoint = res;
            if (!RotatePlayerTowardsMouse) return;
            res.y = caster.owner.Transform.Position.y;
            caster.owner.Transform.LookAt(res);

        }

        public override void CastEnded(CastInfo caster)
        {
            base.CastEnded(caster);
            caster.mousePoint = MousePoint;
            if (caster.castPos == Vector3.zero) caster.castPos = caster.owner.CastPos;
            Ability.Cast(caster);
            if (BlockMovementOnWindup) caster.owner.StartMovement();
            if (BlockRotationOnWindup) caster.owner.StartRotation();
            if (ReducedSpeedOnWindup) caster.owner.Stats.RemoveStatModifier(HalfSpeed);
        }

        [ContextMenu("Delete")]
        protected override void Delete()
        {
            base.Delete();
            DestroyImmediate(Ability, true);
        }
    }
}