using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Character;
using UnityEditor;
using UnityEngine;

public class ConditionDatabaseReferenceEditor : EditorWindow
{
    [MenuItem("Databases/Condition Reference Database")]
    public static void ShowWindow()
    {
        GetWindow<ConditionDatabaseReferenceEditor>("Condition Reference Database");
    }

    ConditionReferenceDatabase database;
    SerializedObject serializedObject;
    bool[] showConditionReference;
    ConditionReferenceData readdCondition;

    private void OnEnable()
    {
        if(database == null)
        {
            database = AssetDatabase.LoadAssetAtPath<ConditionReferenceDatabase>(ConditionReferenceDatabase.DatabasePath);
        }

        if(database == null)
        {
            database = CreateInstance<ConditionReferenceDatabase>();
            AssetDatabase.CreateAsset(database, ConditionReferenceDatabase.DatabasePath);
            AssetDatabase.SaveAssets();
        }

        serializedObject = new SerializedObject(database);
    }

    private void OnGUI()
    {
        serializedObject.Update();
        var conditions = serializedObject.FindProperty("conditions");

        if(GUILayout.Button("Add Condition"))
        {
            conditions.arraySize++;
            var instance = CreateInstance<ConditionReferenceData>();
            AssetDatabase.CreateAsset(instance, AssetDatabase.GenerateUniqueAssetPath(ConditionReferenceDatabase.ConditionsPath + "New Condition.asset"));
            instance.ID = conditions.arraySize;
            conditions.GetArrayElementAtIndex(conditions.arraySize - 1).objectReferenceValue = instance;
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        GUILayout.BeginHorizontal();
        readdCondition = (ConditionReferenceData)EditorGUILayout.ObjectField("Existing condition", readdCondition, typeof(ConditionReferenceData), false);
        if(GUILayout.Button("Add Existing"))
        {
            conditions.arraySize++;
            readdCondition.ID = conditions.arraySize;
            conditions.GetArrayElementAtIndex(conditions.arraySize - 1).objectReferenceValue = readdCondition;
            readdCondition = null;
        }
        GUILayout.EndHorizontal();

        if(showConditionReference == null || showConditionReference.Length != conditions.arraySize)
        {
            showConditionReference = new bool[conditions.arraySize];
        }

        for(int i = 0; i < conditions.arraySize; i++)
        {
            var condition = conditions.GetArrayElementAtIndex(i);
            var conditionReference = condition.objectReferenceValue as ConditionReferenceData;

            GUILayout.BeginHorizontal();
            showConditionReference[i] = EditorGUILayout.Foldout(showConditionReference[i], conditionReference != null ? conditionReference.name : "Condition " + i);
            if(GUILayout.Button("X", GUILayout.Width(20)))
            {
                conditions.DeleteArrayElementAtIndex(i);
                GUILayout.EndHorizontal();
                continue;
            }
            GUILayout.EndHorizontal();

            if (showConditionReference[i])
            {
                EditorGUI.indentLevel++;
                var editor = Editor.CreateEditor(conditionReference);
                editor.OnInspectorGUI();
                EditorGUI.indentLevel--;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}