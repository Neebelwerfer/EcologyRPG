using EcologyRPG.Core.Character;
using EcologyRPG.GameSystems.Abilities.Conditions;
using UnityEditor;

[CustomEditor(typeof(DamageOverTime))]
public class DamageOverTimeDrawer : ConditionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DamageType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damagePerTick"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("tickRate"));
    }
}