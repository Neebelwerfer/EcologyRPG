using Character;
using Character.Abilities;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Utility;

[CreateAssetMenu(fileName = "Sprint", menuName = "Abilities/Sprint")]
public class Sprint : AbilityEffect
{
    public ExhaustionEffect Exhaustion;
    public float sprintSpeedMultiplier = 1f;

    StatUpEffect statUP;

    private void OnValidate()
    {
        if (statUP == null)
        {
            statUP = CreateInstance<StatUpEffect>();
            statUP.StatName = "movementSpeed";
            statUP.ModType = StatModType.PercentMult;
            statUP.Value = sprintSpeedMultiplier;
            statUP.duration = 0.1f;
        }
        statUP.Value = sprintSpeedMultiplier;
    }


    public override void Cast(CastInfo castInfo)
    {
        castInfo.owner.ApplyEffect(castInfo, Instantiate(statUP));
    }
}
