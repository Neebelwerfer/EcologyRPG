using Character;
using Character.Abilities;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Utility;

public class Sprint : BaseAbility
{
    public Exhaustion Exhaustion;
    public float sprintSpeedMultiplier = 1f;

    static SprintCondition sprintCondition;


    public override void Cast(CastInfo castInfo)
    {
        if(sprintCondition == null)
        {
            sprintCondition = new();
            sprintCondition.Value = sprintSpeedMultiplier;
            sprintCondition.duration = 0.25f;
        }


        if(castInfo.owner.Stats.GetResource("Stamina") < 5)
        {
            castInfo.owner.ApplyCondition(castInfo, Instantiate(Exhaustion));
        }

        castInfo.owner.ApplyCondition(castInfo, Instantiate(sprintCondition));
    }
}

public class SprintCondition : BuffCondition
{
    public float Value;
    Stat stat;
    public override void OnApply(CastInfo Caster, BaseCharacter target)
    {
        stat = target.Stats.GetStat("movementSpeed");
        stat.AddModifier(new StatModification("movementSpeed", Value, StatModType.PercentMult, this));
        target.Animator.SetBool("Is_Running", true);
    }

    public override void OnReapply(BaseCharacter target)
    {
        remainingDuration = duration;
    }

    public override void OnRemoved(BaseCharacter target)
    {
        stat.RemoveAllModifiersFromSource(this);
        target.Animator.SetBool("Is_Running", false);
    }

    public override void OnUpdate(BaseCharacter target, float deltaTime)
    {
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Sprint))]
public class SprintEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Sprint ability = (Sprint)target;
        if (ability.Exhaustion == null)
        {
            ability.Exhaustion = CreateInstance<Exhaustion>();
            ability.Exhaustion.name = "Exhaustion";
            AssetDatabase.AddObjectToAsset(ability.Exhaustion, ability);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
            ability.Exhaustion = (Exhaustion)EditorGUILayout.ObjectField("Exhaustion Effect", ability.Exhaustion, typeof(Exhaustion), false);
    }
}
#endif