using EcologyRPG.Core.Character;
using UnityEngine;

namespace EcologyRPG.Core.Abilities.AbilityData
{
    public class AttackAbilityDefinition : AbilityDefintion
    {
        public BaseAbility Ability;
        [HideInInspector] public bool RotatePlayerTowardsMouse = false;

        Vector3 MousePoint;

        public override bool CanActivate(BaseCharacter caster)
        {
            if(!base.CanActivate(caster)) return false;
            return Ability.CanCast(caster);
        }

        public override void CastStarted(CastInfo caster)
        {
            base.CastStarted(caster);

            var res = TargetUtility.GetMousePoint(Camera.main);
            MousePoint = res;
            if(RotatePlayerTowardsMouse)
            {
                res.y = caster.owner.Transform.Position.y;
                Debug.DrawRay(caster.owner.Transform.Position, res - caster.owner.Transform.Position, Color.red, 1f);
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