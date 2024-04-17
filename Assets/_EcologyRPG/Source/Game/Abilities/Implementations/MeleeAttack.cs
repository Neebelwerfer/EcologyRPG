using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities
{
    public class MeleeAttack : WeaponAttack
    {
        public bool showIndicator = false;
        public float width;
        [Min(10)]
        public float angle = 45;
        BaseCharacter[] targets;


        IndicatorMesh indicatorMesh;

        public override void Windup(CastInfo castInfo, float windUpTime)
        {
            base.Windup(castInfo, windUpTime);

            var dir = GetDir(castInfo);

            if (showIndicator)
            {
                if (Physics.Raycast(castInfo.owner.Transform.Position, Vector3.down, out RaycastHit hit, 1000, AbilityManager.WalkableGroundLayer))
                {
                    indicatorMesh = Instantiate(AbilityManager.IndicatorMesh);
                    indicatorMesh.transform.position = hit.point;
                    indicatorMesh.SetColor(castInfo.owner.Faction == Faction.player ? Color.black : Color.red);
                    indicatorMesh.Clear();

                    if (targetType == TargetType.Line)
                        indicatorMesh.TriangulateBox(dir.normalized, Range, width);
                    else if (targetType == TargetType.Cone)
                        indicatorMesh.TriangulateCone(dir.normalized, Range, angle);
                    else if (targetType == TargetType.Circular)
                        indicatorMesh.TriangulateCircle(dir.normalized, Range);

                    indicatorMesh.Apply();
                }
                Destroy(indicatorMesh.gameObject, windUpTime);
            }
        }

        public override void Cast(CastInfo caster)
        {
            var dir = GetDir(caster);

            if (targetType == TargetType.Cone)
            {
                targets = TargetUtility.GetTargetsInCone(caster.castPos, dir, angle, Range, AbilityManager.TargetMask);
            }
            else if (targetType == TargetType.Line)
            {
                targets = TargetUtility.GetTargetsInLine(caster.castPos, dir, new Vector3(width / 2, 10, Range / 2), AbilityManager.TargetMask);
            }
            else if (targetType == TargetType.Circular)
            {
                targets = TargetUtility.GetTargetsInRadius(caster.castPos, Range, AbilityManager.TargetMask);
            }

            foreach (var target in targets)
            {
                if (target != null && target.Faction != caster.owner.Faction)
                {
                    DefaultOnHitAction()(caster, target);
                }
            }
        }
    }
}
