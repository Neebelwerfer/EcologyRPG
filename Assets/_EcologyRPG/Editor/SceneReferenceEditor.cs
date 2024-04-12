using EcologyRPG.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(SceneReference))]
public class SceneReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var sceneReference = (SceneReference)target;
        if(sceneReference == null)
        {
            return;
        }
        EditorGUILayout.LabelField("Path: ", AssetDatabase.GetAssetPath(serializedObject.FindProperty("sceneAsset").objectReferenceValue));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sceneAsset"));
        var buildIndex = sceneReference.BuildIndex;
        EditorGUILayout.LabelField("Build Index: ",buildIndex.ToString());

        if(buildIndex < 0)
        {
            if (GUILayout.Button("Add to Build Settings"))
            {
                EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
                EditorBuildSettingsScene[] newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
                for (int j = 0; j < buildScenes.Length; j++)
                {
                    newBuildScenes[j] = buildScenes[j];
                }
                newBuildScenes[buildScenes.Length] = new EditorBuildSettingsScene(AssetDatabase.GetAssetPath(sceneReference.sceneAsset), true);
                EditorBuildSettings.scenes = newBuildScenes;
            }
        }
        else
        {
            if (GUILayout.Button("Remove from build settings"))
            {
                EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
                EditorBuildSettingsScene[] newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length - 1];
                int j = 0;
                for (int k = 0; k < buildScenes.Length; k++)
                {
                    if (buildScenes[k].path != AssetDatabase.GetAssetPath(sceneReference.sceneAsset))
                    {
                        newBuildScenes[j] = buildScenes[k];
                        j++;
                    }
                }
                EditorBuildSettings.scenes = newBuildScenes;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}