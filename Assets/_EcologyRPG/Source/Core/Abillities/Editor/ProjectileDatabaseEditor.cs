using EcologyRPG.AbilityScripting;
using UnityEditor;
using UnityEngine;

public class ProjectileDatabaseEditor : EditorWindow
{
    SerializedObject serializedObject;

    [MenuItem("Game/Projectile Database")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ProjectileDatabaseEditor));
    }

    private void OnEnable()
    {
        var database = AssetDatabase.LoadAssetAtPath<ProjectileDatabase>(ProjectileDatabase.ResourceFullPath);
        if(database == null)
        {
            database = CreateInstance<ProjectileDatabase>();
            AssetDatabase.CreateAsset(database, ProjectileDatabase.ResourceFullPath);
        }
        serializedObject = new SerializedObject(database);
    }

    void OnGUI()
    {
        var list = serializedObject.FindProperty("projectiles");
        for (int i = 0; i < list.arraySize; i++)
        {
            var element = list.GetArrayElementAtIndex(i);
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(element);
            if (GUILayout.Button("X"))
            {
                list.DeleteArrayElementAtIndex(i);
            }
            GUILayout.EndHorizontal();
        }
        
        if(GUILayout.Button("Add Projectile"))
        {
            list.arraySize++;
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Discard Changes"))
        {
            serializedObject.Update();
        }
        GUILayout.EndHorizontal();
    }

}