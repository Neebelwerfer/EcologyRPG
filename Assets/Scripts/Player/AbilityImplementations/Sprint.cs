using Character;
using Character.Abilities;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

[CreateAssetMenu(fileName = "Sprint", menuName = "Abilities/Sprint")]
public class Sprint : BaseAbility
{
    readonly StatModification sprintSpeed;

    Resource stamina;

    public Sprint()
    {
        sprintSpeed = new StatModification("movementSpeed", 1f, StatModType.PercentMult, this);
    }
    public override void CastStarted(CasterInfo caster)
    {
        caster.owner.Stats.AddStatModifier(sprintSpeed);
        stamina = caster.owner.Stats.GetResource(ResourceName);
    }

    public override void OnHold(CasterInfo caster)
    {
        if (stamina < ResourceCost * TimeManager.IngameDeltaTime)
        {
            caster.owner.ApplyCharacterModification(new Exhaustion(caster.activationInput, stamina));
        }

        stamina -= ResourceCost * TimeManager.IngameDeltaTime;
    }

    public override void CastEnded(CasterInfo caster)
    {
        caster.owner.Stats.RemoveStatModifier(sprintSpeed);
    }
}

public class Exhaustion : CharacterEffect
{
    readonly InputAction sprintInput;
    readonly Resource Stamina;

    public Exhaustion(InputAction input, Resource stamina) : base("Exhausted", 100000, EffectType.Debuff)
    {
        sprintInput = input;
        Stamina = stamina;
    }
    public override void OnApply(BaseCharacter target)
    {
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
