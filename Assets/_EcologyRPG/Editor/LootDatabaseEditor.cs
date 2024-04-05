using EcologyRPG.Core.Items;
using UnityEditor;
using UnityEngine;

public class LootDatabaseEditor : EditorWindow
{
    [MenuItem("Game/Loot Settings")]
    public static void ShowWindow()
    {
        GetWindow<LootDatabaseEditor>("Loot Settings");
    }

    LootDatabase LootDatabase;
    bool[] categoryFoldouts;
    SerializedObject lootDb;
    string CategorySearch = "";
    Vector2 scrollPos;

    private void OnEnable()
    {
        LootDatabase = AssetDatabase.LoadAssetAtPath<LootDatabase>("Assets/_EcologyRPG/Resources/Config/Loot Database.asset");
        categoryFoldouts = new bool[LootDatabase.CategoryOdds.Count];        
        lootDb = new SerializedObject(LootDatabase);
    }
    private void OnGUI()
    {
        if (LootDatabase == null) return;
        GUILayout.Label("Loot Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(lootDb.FindProperty(nameof(LootDatabase.lootChance)));
        EditorGUILayout.PropertyField(lootDb.FindProperty(nameof(LootDatabase.minLootAmount)));
        EditorGUILayout.PropertyField(lootDb.FindProperty(nameof(LootDatabase.maxLootAmount)));
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        CategorySearch = EditorGUILayout.TextField("Category Search", CategorySearch);

        var categories = lootDb.FindProperty(nameof(LootDatabase.CategoryOdds));
        if (categoryFoldouts.Length != categories.arraySize)
        {
            categoryFoldouts = new bool[categories.arraySize];
        }

        int indexToDelete = -1;
        for (int i = 0; i < categories.arraySize; i++)
        {
            var categoryObj = categories.GetArrayElementAtIndex(i);
            var categoryName = categoryObj.FindPropertyRelative(nameof(CategoryOdds.category)).stringValue;
            if (!categoryName.ToLower().Contains(CategorySearch.ToLower())) continue;
            EditorGUILayout.BeginHorizontal();
            categoryFoldouts[i] = EditorGUILayout.Foldout(categoryFoldouts[i], categoryName);
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                indexToDelete = i;
            }
            EditorGUILayout.EndHorizontal();
            if (categoryFoldouts[i])
            {
                EditorGUILayout.PropertyField(lootDb.FindProperty(nameof(LootDatabase.CategoryOdds)).GetArrayElementAtIndex(i).FindPropertyRelative(nameof(CategoryOdds.category)));
                EditorGUILayout.PropertyField(lootDb.FindProperty(nameof(LootDatabase.CategoryOdds)).GetArrayElementAtIndex(i).FindPropertyRelative(nameof(CategoryOdds.weight)));
                EditorGUILayout.PropertyField(lootDb.FindProperty(nameof(LootDatabase.CategoryOdds)).GetArrayElementAtIndex(i).FindPropertyRelative(nameof(CategoryOdds.items)));
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }

        if (GUILayout.Button("Add Category"))
        {
            categories.InsertArrayElementAtIndex(categories.arraySize);
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            lootDb.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Discard Changes"))
        {
            DiscardChanges();
            lootDb.Update();
            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        if (indexToDelete != -1)
        {
            categories.DeleteArrayElementAtIndex(indexToDelete);
        }
    }
}