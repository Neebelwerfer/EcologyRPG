using UnityEditor;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SlowCondition))]
    public class SlowConditionEditor : ConditionEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SlowCondition ability = (SlowCondition)target;
            ability.slowType = (SlowCondition.SlowType)EditorGUILayout.EnumPopup("Slow Type", ability.slowType);
            if (ability.slowType == SlowCondition.SlowType.Flat)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SlowAmount"));
            else if (ability.slowType == SlowCondition.SlowType.Decaying)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SlowCurve"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}

