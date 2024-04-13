using EcologyRPG.GameSystems.Interactables;
using UnityEditor;

[CustomEditor(typeof(LootSpawner))]
public class LootSpawnerDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        var lootType = serializedObject.FindProperty("lootType");
        var loot = serializedObject.FindProperty("loot");
        var tags = serializedObject.FindProperty("tags");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("OneTimeUse"));
        EditorGUILayout.PropertyField(lootType);

        if(lootType.enumValueIndex == 0)
        {
            EditorGUILayout.PropertyField(loot);
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minLoot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxLoot"));
            EditorGUILayout.PropertyField(tags);
        }

        serializedObject.ApplyModifiedProperties();
    }
}