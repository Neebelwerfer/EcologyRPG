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

    public Sprint()
    {
        sprintSpeed = new StatModification(1f, StatModType.PercentMult, this);
    }
    public override IEnumerator Cast(CasterInfo caster)
    {
        var stamina = caster.owner.stats.GetResource(ResourceName);
        caster.owner.stats.AddStatModifier("movementSpeed", sprintSpeed);

        while(caster.activationInput.action.IsPressed())
        {
            if (stamina < ResourceCost)
            {
                caster.owner.ApplyDebuff(new NoSprint(caster.activationInput, stamina));
                break;
            }

            stamina -= ResourceCost * TimeManager.IngameDeltaTime;
            yield return null;
        }

        caster.owner.stats.RemoveStatModifier("movementSpeed", sprintSpeed);
    }
}

public class NoSprint : Debuff
{
    readonly InputActionReference sprintInput;
    readonly Resource Stamina;

    public NoSprint(InputActionReference input, Resource stamina) : base(100000)
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
        if(Stamina.CurrentValue == Stamina.MaxValue)
        {
            remainingDuration = 0;
        }
    }
}
