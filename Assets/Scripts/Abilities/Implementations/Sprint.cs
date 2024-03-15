using Character;
using Character.Abilities;
using System.Collections;
using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = "Sprint", menuName = "Abilities/Sprint")]
public class Sprint : AbilityEffect
{
    public ExhaustionEffect Exhaustion;
    public float sprintSpeedMultiplier = 1f;
    readonly StatModification sprintSpeed;

    Resource stamina;

    public Sprint()
    {
        sprintSpeed = new StatModification("movementSpeed", sprintSpeedMultiplier, StatModType.PercentMult, this);
    }

    public override void Cast(CastInfo castInfo)
    {

    }
}
