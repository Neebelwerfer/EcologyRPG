using EcologyRPG.Core.Abilities;
using EcologyRPG.GameSystems.Abilities.Conditions;

namespace EcologyRPG.GameSystems.Abilities
{
    public class ChargeAttack : AttackAbility
    {
        public DashCondition dash;

        public override void Cast(CastInfo castInfo)
        {
            base.Cast(castInfo);
            if (useMouseDirection)
            {
                dash.directionMode = DirectionMode.Mouse;
            }
            else
            {
                dash.directionMode = DirectionMode.Movement;
            }
            dash.dashRange = Range;
            dash.OnFirstHitEffects = OnFirstHit;
            dash.OnHitEffects = OnHitEffects;

            castInfo.owner.ApplyCondition(castInfo, Instantiate(dash));
        }
    }
}
