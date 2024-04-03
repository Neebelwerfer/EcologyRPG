using EcologyRPG._Core.Abilities;
using EcologyRPG._Core.Character;
using UnityEngine.InputSystem;

namespace EcologyRPG._Game.Abilities.Conditions
{
    public class Exhaustion : DebuffCondition
    {

        InputAction sprintInput;
        Resource Stamina;



        public override void OnApply(CastInfo caster, BaseCharacter target)
        {
            Stamina = target.Stats.GetResource("Stamina");
            sprintInput = caster.activationInput;

            sprintInput.Disable();
        }

        public override void OnReapply(BaseCharacter target)
        {

        }

        public override void OnRemoved(BaseCharacter target)
        {
            sprintInput.Enable();
        }

        public override void OnUpdate(BaseCharacter target, float deltaTime)
        {
            if (Stamina.CurrentValue > Stamina.MaxValue * 0.75)
            {
                remainingDuration = 0;
            }
        }
    }
}
