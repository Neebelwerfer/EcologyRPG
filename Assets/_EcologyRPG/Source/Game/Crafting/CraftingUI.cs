using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.GameSystems.Crafting
{
    public class CraftingUI : MonoBehaviour
    {
        public static CraftingUI Instance;
        public GameObject RecipeButtonPrefab;
        public GameObject CraftingButtonArea;

        RecipeDatabase recipeDatabase;

        public List<string> blockedCrafts;

        RecipeButton[] recipeButtons;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Setup()
        {
            blockedCrafts = new List<string>();
            recipeDatabase = Resources.Load<RecipeDatabase>(RecipeDatabase.ResourcePath);
            recipeDatabase.Sort();
            recipeButtons = new RecipeButton[recipeDatabase.Recipes.Length];
            Debug.Log($"Loaded {recipeDatabase.Recipes.Length} recipes");
        }

        private void OnEnable()
        {
            if(recipeDatabase == null)
            {
                Setup();
            }

            int j = 0;
            for (int i = 0; i < recipeDatabase.Recipes.Length; i++)
            {
                var recipe = recipeDatabase.Recipes[i];
                if (blockedCrafts.Contains(recipe.Name)) continue;
                var buttonObject = Instantiate(RecipeButtonPrefab, CraftingButtonArea.transform);
                RecipeButton button = buttonObject.GetComponent<RecipeButton>();
                button.Setup(recipe);
                recipeButtons[j] = button;
                j++;
            }
        }

        private void OnDisable()
        {
            foreach (RecipeButton button in recipeButtons)
            {
                if(button == null) continue;
                Destroy(button.gameObject);
            }
        }
    }
}