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
        sprintSpeed = new StatModification(1f, StatModType.PercentMult, this);
    }
    public override void CastStarted(CasterInfo caster)
    {
        caster.owner.stats.AddStatModifier("movementSpeed", sprintSpeed);
        stamina = caster.owner.stats.GetResource(ResourceName);
    }

    public override void OnHold(CasterInfo caster)
    {
        if (stamina < ResourceCost * TimeManager.IngameDeltaTime)
        {
            caster.owner.ApplyCharacterModification(new NoSprint(caster.activationInput, stamina));
        }

        stamina -= ResourceCost * TimeManager.IngameDeltaTime;
    }

    public override void CastEnded(CasterInfo caster)
    {
        caster.owner.stats.RemoveStatModifier("movementSpeed", sprintSpeed);
    }
}

public class NoSprint : CharacterModification
{
    readonly InputActionReference sprintInput;
    readonly Resource Stamina;

    public NoSprint(InputActionReference input, Resource stamina) : base("Exhausted", 100000, CharacterModificationType.Debuff)
    {
        sprintInput = input;
        Stamina = stamina;
    }
    public override void OnApply(BaseCharacter target)
    {
        sprintInput.action.Disable();
    }

    public override void OnRemoved(BaseCharacter target)
    {
        sprintInput.action.Enable();
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
        if(Stamina.CurrentValue > Stamina.MaxValue * 0.75)
        {
            remainingDuration = 0;
        }
    }
}
