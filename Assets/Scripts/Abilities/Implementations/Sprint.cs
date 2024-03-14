using Character;
using Character.Abilities;
using System.Collections;
using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = "Sprint", menuName = "Abilities/Sprint")]
public class Sprint : BaseAbility
{
    public ExhaustionEffect Exhaustion;
    public float sprintSpeedMultiplier = 1f;
    readonly StatModification sprintSpeed;

    Resource stamina;

    public Sprint()
    {
        sprintSpeed = new StatModification("movementSpeed", sprintSpeedMultiplier, StatModType.PercentMult, this);
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
            caster.owner.ApplyEffect(caster, Instantiate(Exhaustion));
        }

        stamina -= ResourceCost * TimeManager.IngameDeltaTime;
    }

    public override void CastEnded(CasterInfo caster)
    {
        caster.owner.Stats.RemoveStatModifier(sprintSpeed);
    }
}
