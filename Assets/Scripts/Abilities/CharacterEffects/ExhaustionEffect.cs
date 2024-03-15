using Character;
using Character.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ExhaustionEffect", menuName = DebuffPath + "ExhaustionEffect")]
public class ExhaustionEffect : DebuffEffect
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
        throw new System.NotImplementedException();
    }

    public override void OnRemoved(BaseCharacter target)
    {
        sprintInput.Enable();
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        if(Stamina.CurrentValue > Stamina.MaxValue * 0.75)
        {
            remainingDuration = 0;
        }
    }
}
