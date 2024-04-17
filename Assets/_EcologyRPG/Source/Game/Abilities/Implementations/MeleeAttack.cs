using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Abilities
{
    public class MeleeAttack : WeaponAttack
    {
        public float width;
        [Min(10)]
        public float angle = 45;
        BaseCharacter[] targets;


        IndicatorMesh indicatorMesh;

        public override void Windup(CastInfo castInfo, float windUpTime)
        {
            base.Windup(castInfo, windUpTime);

            var dir = GetDir(castInfo);
            
            if(targetType == TargetType.Line)
            {
                if (Physics.Raycast(castInfo.owner.Transform.Position, Vector3.down, out RaycastHit hit, 1000, AbilityManager.GroundMask))
                {
                    indicatorMesh = Instantiate(AbilityManager.IndicatorMesh);
                    indicatorMesh.transform.position = hit.point;
                    indicatorMesh.Clear();

                    indicatorMesh.TriangulateBox(dir.normalized, Range, width);
                    indicatorMesh.Apply();
                }
            }
            if(targetType == TargetType.Cone)
            {
                if (Physics.Raycast(castInfo.owner.Transform.Position, Vector3.down, out RaycastHit hit, 1000, AbilityManager.GroundMask))
                {
                    
                    indicatorMesh = Instantiate(AbilityManager.IndicatorMesh);
                    indicatorMesh.transform.position = hit.point;
                    indicatorMesh.Clear();

                    indicatorMesh.TriangulateCone(dir.normalized, Range, angle);
                    indicatorMesh.Apply();
                }
            }
            Destroy(indicatorMesh.gameObject, windUpTime); ;
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
