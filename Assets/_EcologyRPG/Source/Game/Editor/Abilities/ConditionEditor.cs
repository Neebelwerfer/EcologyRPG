using EcologyRPG.Core.Character;
using UnityEditor;

    [CustomEditor(typeof(Condition), true)]
    public class ConditionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var condition = (Condition)target;
            EditorGUILayout.LabelField("Condition Settings", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("ID", condition.ID);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
            serializedObject.ApplyModifiedProperties();
        }
    }
