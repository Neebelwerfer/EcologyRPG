using EcologyRPG.Game.Abilities.Conditions;
using EcologyRPG.Game.Abilities.Implementations;
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
        ability.dash.StopOnHit = EditorGUILayout.Toggle("Stop On Hit", ability.dash.StopOnHit);
        ability.dash.duration = EditorGUILayout.FloatField("Dash Time", ability.dash.duration);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif