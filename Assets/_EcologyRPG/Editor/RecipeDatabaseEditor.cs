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
        foldouts = new bool[database.Recipes.Length];
    }

    private void OnGUI()
    {
        GUILayout.Label("Recipe Database", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        newRecipe = EditorGUILayout.ObjectField(newRecipe, typeof(Recipe), false) as Recipe;
        if (GUILayout.Button("Add Recipe") && newRecipe != null)
        {
            database.Add(newRecipe);
            newRecipe = null;
            AssetDatabase.SaveAssets();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        search = EditorGUILayout.TextField("Search", search);
        GUILayout.Space(10);

        var recipes = database.Recipes;
        if (foldouts.Length != database.Recipes.Length)
        {
            foldouts = new bool[database.Recipes.Length];
        }
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        var recipeCount = recipes.Length;
        for (int i = 0; i < recipeCount; i++)
        {
            var recipe = recipes[i];
            if (!string.IsNullOrEmpty(search) && !recipe.Name.ToLower().Contains(search.ToLower()))
            {
                continue;
            }

            GUILayout.BeginHorizontal();
            foldouts[i] = EditorGUILayout.Foldout(foldouts[i], recipe.name);
            if (GUILayout.Button("X"))
            {
                database.Remove(recipe);
                AssetDatabase.SaveAssets();
            }
            GUILayout.EndHorizontal();
            if (foldouts[i])
            {
                var e = Editor.CreateEditor(recipe);
                e.OnInspectorGUI();
            }
        }
        GUILayout.EndScrollView();
    }
}