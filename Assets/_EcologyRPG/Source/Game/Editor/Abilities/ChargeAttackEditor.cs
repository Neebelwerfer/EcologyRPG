using EcologyRPG.GameSystems.Abilities;
using EcologyRPG.GameSystems.Abilities.Conditions;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ChargeAttack))]
public class ChargeAttackEditor : AttackAbilityEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ChargeAttack ability = (ChargeAttack)target;

        if(ability.dash == null)
        {
            ability.dash = CreateInstance<DashCondition>();
            AssetDatabase.AddObjectToAsset(ability.dash, ability);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        EditorGUILayout.LabelField("Dash Settings", EditorStyles.boldLabel);
        var stop = EditorGUILayout.Toggle("Stop On Hit", ability.dash.StopOnHit);
        var duration = EditorGUILayout.FloatField("Dash Time", ability.dash.duration);

        if(stop != ability.dash.StopOnHit || duration != ability.dash.duration)
        {
            ability.dash.StopOnHit = stop;
            ability.dash.duration = duration;
            EditorUtility.SetDirty(ability.dash);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif