using UnityEditor;

namespace EcologyRPG.GameSystems.Abilities.Conditions
{
#if UNITY_EDITOR
    [CustomEditor(typeof(StatDown))]
    public class StatDownEditor : ConditionEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            StatDown ability = (StatDown)target;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("StatName"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ModType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Value"));
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}