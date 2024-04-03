using EcologyRPG.Core.Character;
using EcologyRPG.Game.Abilities.Conditions;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(DashCondition))]
public class DashConditionEditor : ConditionEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DashCondition ability = (DashCondition)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("directionMode"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("dashRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("StopOnHit"));
        AbilityComponentEditor.Display("On First Hit Components", ability.OnFirstHitEffects, ability, DisplayComponentType.All);
        AbilityComponentEditor.Display("On Hit Components", ability.OnHitEffects, ability, DisplayComponentType.All);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif