using UnityEditor;
using static WeaponAttack;
#if UNITY_EDITOR
[CustomEditor(typeof(MeleeAttack))]
public class MeleeAttackEditor : WeaponAttackEditor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MeleeAttack ability = (MeleeAttack)target;
        if(ability.targetType == TargetType.Cone)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("angle"));
        else if(ability.targetType == TargetType.Line)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("width"));

        serializedObject.ApplyModifiedProperties();
    }
}
#endif