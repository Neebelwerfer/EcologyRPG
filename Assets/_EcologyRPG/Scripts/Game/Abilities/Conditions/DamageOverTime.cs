using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;


namespace EcologyRPG.GameSystems.Abilities.Conditions
{
    public class DamageOverTime : DebuffCondition
    {
        public float damagePerTick = 10;
        public DamageType DamageType;
        public float tickRate = 1;

        private float timeSinceLastTick = 0;

        private float _BaseDamagePerTick;

        public override void OnApply(CastInfo caster, BaseCharacter target)
        {
            timeSinceLastTick = 0;
            _BaseDamagePerTick = damagePerTick;
        }

        public override void OnReapply(BaseCharacter target)
        {
            var remainingTicks = remainingDuration / tickRate;
            var rollOverDamage = (remainingTicks * damagePerTick) * (tickRate / duration);
            _BaseDamagePerTick += rollOverDamage;
            remainingDuration = duration;
        }

        public override void OnRemoved(BaseCharacter target)
        {
            var mult = 1 - (timeSinceLastTick / tickRate);
            if (mult < 0.1) return;

            target.ApplyDamage(CalculateDamage(Owner, DamageType, _BaseDamagePerTick * mult));
        }

        public override void OnUpdate(BaseCharacter target, float deltaTime)
        {
            timeSinceLastTick += deltaTime;
            if (timeSinceLastTick >= tickRate)
            {
                timeSinceLastTick = 0;
                target.ApplyDamage(CalculateDamage(Owner, DamageType, _BaseDamagePerTick));
            }
        }
    }
}