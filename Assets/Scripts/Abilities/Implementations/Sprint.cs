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

    static StatUp statUP;


    public override void Cast(CastInfo castInfo)
    {
        if(statUP == null)
        {
            statUP = CreateInstance<StatUp>();
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