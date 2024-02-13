using Character;
using Character.Abilities;
using System.Collections;
using Utility;

public class Sprint : BaseAbility
{
    StatModification sprintSpeed;
    public Sprint()
    {
        name = "Sprint";
        ResourceCost = 25;
        ResourceName = "stamina";
        Cooldown = 0;
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