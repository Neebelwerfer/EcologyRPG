using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEngine.InputSystem;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{
    public class Exhaustion : DebuffCondition, IUpdateCondition
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
