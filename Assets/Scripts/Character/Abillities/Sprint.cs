using Character;
using Character.Abilities;
using System.Collections;
using UnityEngine;
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
            if (stamina < ResourceCost) break;
            stamina -= ResourceCost * TimeManager.IngameDeltaTime;
            yield return null;
        }

        caster.owner.stats.RemoveStatModifier("movementSpeed", sprintSpeed);
    }
}