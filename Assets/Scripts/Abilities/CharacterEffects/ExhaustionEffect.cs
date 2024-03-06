using Character;
using Character.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ExhaustionEffect", menuName = "Abilities/CharacterEffects/ExhaustionEffect")]
public class ExhaustionEffect : CharacterEffect
{

    InputAction sprintInput;
    Resource Stamina;



    public override void OnApply(CasterInfo caster, BaseCharacter target)
    {
        Stamina = target.Stats.GetResource("Stamina");
        sprintInput = caster.activationInput;

        sprintInput.Disable();
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
