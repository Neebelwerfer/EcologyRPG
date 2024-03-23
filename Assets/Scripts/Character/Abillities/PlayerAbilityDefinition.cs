using Character.Abilities;
using Character.Attributes;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Player ability data", menuName = "Ability/Player Ability Data")]
public class PlayerAbilityDefinition : AttackAbilityDefinition
{
    [Header("Resources")]
    [Tooltip("The resource that get used for the ability cost"), StatAttribute(StatType.Resource)]
    public string ResourceName;
    [Tooltip("The resource cost of this ability")]
    public float ResourceCost = 0;
    [Tooltip("The description of the ability"), TextArea(1, 5)]
    public string Description;

    public override bool CanActivate(CastInfo caster)
    {
        if (!base.CanActivate(caster)) return false;
        if (ResourceName != "" && caster.owner.Stats.GetResource(ResourceName) < ResourceCost)
        {
            Debug.Log("Not enough resource");
            return false;
        }
        return true;
    }

    public override IEnumerator HandleCast(CastInfo caster)
    {
        if (ResourceCost > 0)
        {
            InitialCastCost(caster);
        }
        return base.HandleCast(caster);
    }

    /// <summary>
    /// Called when the cast is started to deduct the resource cost
    /// </summary>
    /// <param name="caster"></param>
    protected virtual void InitialCastCost(CastInfo caster)
    {
        var resource = caster.owner.Stats.GetResource(ResourceName);
        resource -= ResourceCost;
    }
}

#if UNITY_EDITOR
#endif