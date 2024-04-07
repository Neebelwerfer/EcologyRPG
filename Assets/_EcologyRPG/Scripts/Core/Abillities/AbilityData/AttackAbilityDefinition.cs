using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Abilities.AbilityData
{
    public class AttackAbilityDefinition : AbilityDefintion
    {
        public BaseAbility Ability;
        public bool RotatePlayerTowardsMouse = false;

        Vector3 MousePoint;

        public override void CastStarted(CastInfo caster)
        {
            base.CastStarted(caster);

            var res = TargetUtility.GetMousePoint(Camera.main);
            MousePoint = res;
            if(RotatePlayerTowardsMouse)
            {
                res.y = caster.owner.Transform.Position.y;
                caster.owner.Transform.LookAt(res);
            }
            else if (caster.dir != Vector3.zero)
            {
                caster.owner.Transform.LookAt(caster.dir);
            }
        }

        public override void CastCancelled(CastInfo caster)
        {
            base.CastCancelled(caster);

        }

        public override void CastFinished(CastInfo caster)
        {
            base.CastFinished(caster);
            caster.targetPoint = MousePoint;
            if (caster.castPos == Vector3.zero) caster.castPos = caster.owner.CastPos;
            Ability.Cast(caster);
        }

        [ContextMenu("Delete")]
        protected override void Delete()
        {
            base.Delete();
            DestroyImmediate(Ability, true);
        }
    }
}