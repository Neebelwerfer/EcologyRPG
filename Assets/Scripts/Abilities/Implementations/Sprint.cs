using Character;
using Character.Abilities;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Utility;

public class Sprint : BaseAbility
{
    public ExhaustionEffect Exhaustion;
    public float sprintSpeedMultiplier = 1f;

    static StatUpEffect statUP;


    public override void Cast(CastInfo castInfo)
    {
        if(statUP == null)
        {
            statUP = CreateInstance<StatUpEffect>();
            statUP.StatName = "movementSpeed";
            statUP.ModType = StatModType.PercentMult;
            statUP.Value = sprintSpeedMultiplier;
            statUP.duration = 0.25f;
        }

        if(castInfo.owner.Stats.GetResource("Stamina") < 5)
        {
            castInfo.owner.ApplyEffect(castInfo, Instantiate(Exhaustion));
        }

        castInfo.owner.ApplyEffect(castInfo, Instantiate(statUP));
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
            ability.Exhaustion = CreateInstance<ExhaustionEffect>();
            ability.Exhaustion.name = "Exhaustion";
            AssetDatabase.AddObjectToAsset(ability.Exhaustion, ability);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
            ability.Exhaustion = (ExhaustionEffect)EditorGUILayout.ObjectField("Exhaustion Effect", ability.Exhaustion, typeof(ExhaustionEffect), false);
    }
}
#endif