using EcologyRPG.Core.Items;
using EcologyRPG.GameSystems.Crafting;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RecipeDatabaseEditor : EditorWindow
{
    RecipeDatabase database;
    bool[] foldouts = new bool[0];
    Recipe newRecipe;
    string search = "";
    Vector2 scrollPosition;
    SerializedObject serializedObject;

    [MenuItem("Game/Recipe Database")]
    public static void ShowWindow()
    {
        GetWindow<RecipeDatabaseEditor>("Recipe Database Editor");
    }

    private void OnEnable()
    {
        database = AssetDatabase.LoadAssetAtPath<RecipeDatabase>(RecipeDatabase.DatabasePath);
        if(database == null)
        {
            database = CreateInstance<RecipeDatabase>();
            AssetDatabase.CreateAsset(database, RecipeDatabase.DatabasePath);
            AssetDatabase.SaveAssets();
        }
        serializedObject = new SerializedObject(database);
        foldouts = new bool[database.Recipes.Length];
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        var recipes = serializedObject.FindProperty("Recipes");

        newRecipe = EditorGUILayout.ObjectField(newRecipe, typeof(Recipe), false) as Recipe;
        if (GUILayout.Button("Add Recipe") && newRecipe != null)
        {
            recipes.InsertArrayElementAtIndex(recipes.arraySize);
            recipes.GetArrayElementAtIndex(recipes.arraySize - 1).objectReferenceValue = newRecipe;
            newRecipe = null;
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        search = EditorGUILayout.TextField("Search", search);
        GUILayout.Space(10);

        var recipeCount = recipes.arraySize;
        if (foldouts.Length != recipeCount)
        {
            foldouts = new bool[recipeCount];
        }
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (int i = 0; i < recipeCount; i++)
        {
            var recipe = recipes.GetArrayElementAtIndex(i).objectReferenceValue as Recipe;

            if (!string.IsNullOrEmpty(search) && !recipe.Name.ToLower().Contains(search.ToLower()))
            {
                continue;
            }

            GUILayout.BeginHorizontal();
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], recipe.Name);
            if (GUILayout.Button("X"))
            {
                recipes.DeleteArrayElementAtIndex(i);
                AssetDatabase.SaveAssets();
            }
            GUILayout.EndHorizontal();

            if (foldouts[i])
            {
                var editor = Editor.CreateEditor(recipe);
                editor.OnInspectorGUI();
            }
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            serializedObject.ApplyModifiedProperties();
        }
        if (GUILayout.Button("Cancel"))
        {
            serializedObject.Update();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }
}