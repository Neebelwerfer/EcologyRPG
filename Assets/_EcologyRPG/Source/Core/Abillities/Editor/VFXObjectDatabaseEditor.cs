using EcologyRPG.Core.Abilities;
using UnityEditor;

public class VFXObjectDatabaseEditor : EditorWindow
{
    [MenuItem("Game/VFX Object Database")]
    public static void ShowWindow()
    {
        GetWindow<VFXObjectDatabaseEditor>("VFX Object Database");
    }

    SerializedObject serializedObject;

    private void OnEnable()
    {
        if(VFXObjectDatabase.Instance == null)
        {
            VFXObjectDatabase.Load();
        }
        if(VFXObjectDatabase.Instance == null)
        {
            VFXObjectDatabase.Instance = CreateInstance<VFXObjectDatabase>();
            AssetDatabase.CreateAsset(VFXObjectDatabase.Instance, VFXObjectDatabase.ResourceFullPath);
        }
        serializedObject = new SerializedObject(VFXObjectDatabase.Instance);
    }

    private void OnGUI()
    {
        serializedObject.Update();
        SerializedProperty prop = serializedObject.FindProperty("vfxObjects");
        EditorGUILayout.PropertyField(prop, true);
        serializedObject.ApplyModifiedProperties();
    }
}