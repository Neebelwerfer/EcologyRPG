using EcologyRPG.GameSystems.Abilities;
using UnityEditor;
using static EcologyRPG.Core.Abilities.WeaponAttack;

#if UNITY_EDITOR
[CustomEditor(typeof(MeleeAttack))]
public class MeleeAttackEditor : WeaponAttackEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeleeAttack ability = (MeleeAttack)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("showIndicator"));
        if(ability.targetType == TargetType.Cone)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("angle"));
        else if(ability.targetType == TargetType.Line)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("width"));

        if(serializedObject.FindProperty("angle").floatValue > 180)
            serializedObject.FindProperty("angle").floatValue = 180;

        serializedObject.ApplyModifiedProperties();
    }
}
#endif