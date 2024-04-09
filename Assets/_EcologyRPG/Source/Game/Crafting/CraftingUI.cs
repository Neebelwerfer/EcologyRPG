using UnityEngine;

namespace EcologyRPG.GameSystems.Crafting
{
    public class CraftingUI : MonoBehaviour
    {
        public GameObject RecipeButtonPrefab;
        public GameObject CraftingButtonArea;

        RecipeDatabase recipeDatabase;

        RecipeButton[] recipeButtons;

        void Setup()
        {
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

            for (int i = 0; i < recipeDatabase.Recipes.Length; i++)
            {
                var recipe = recipeDatabase.Recipes[i];
                var buttonObject = Instantiate(RecipeButtonPrefab, CraftingButtonArea.transform);
                RecipeButton button = buttonObject.GetComponent<RecipeButton>();
                button.Setup(recipe);
                recipeButtons[i] = button;
            }
        }

        private void OnDisable()
        {
            foreach (RecipeButton button in recipeButtons)
            {
                Destroy(button.gameObject);
            }
        }
    }
}