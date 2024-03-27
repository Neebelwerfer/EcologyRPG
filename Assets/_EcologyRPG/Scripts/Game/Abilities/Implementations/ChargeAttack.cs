using EcologyRPG.Core.Abilities;
using EcologyRPG.Game.Abilities.Conditions;
using EcologyRPG.Game.Abilities.Definitions;

namespace EcologyRPG.Game.Abilities.Implementations
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
