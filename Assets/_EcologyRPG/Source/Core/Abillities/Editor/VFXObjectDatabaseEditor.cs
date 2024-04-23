using EcologyRPG.Core.Abilities;
using UnityEditor;

public class VFXObjectDatabaseEditor : EditorWindow
{
    [MenuItem("Game/VFX Object Database")]
    public static void ShowWindow()
    {
        GetWindow<VFXObjectDatabaseEditor>("VFX Database");
    }

    SerializedObject serializedObject;

    private void OnEnable()
    {
        if(VFXDatabase.Instance == null)
        {
            VFXDatabase.Load();
        }
        if(VFXDatabase.Instance == null)
        {
            VFXDatabase.Instance = CreateInstance<VFXDatabase>();
            AssetDatabase.CreateAsset(VFXDatabase.Instance, VFXDatabase.ResourceFullPath);
        }
        serializedObject = new SerializedObject(VFXDatabase.Instance);
    }

    private void OnGUI()
    {
        serializedObject.Update();
        SerializedProperty prop = serializedObject.FindProperty("vfxObjects");
        EditorGUILayout.PropertyField(prop, true);
        serializedObject.ApplyModifiedProperties();
    }
}