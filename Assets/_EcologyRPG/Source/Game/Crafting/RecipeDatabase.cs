using System;
using UnityEngine;

namespace EcologyRPG.GameSystems.Crafting
{
    public class RecipeDatabase : ScriptableObject
    {
        public const string DatabasePath = "Assets/_EcologyRPG/Resources/Recipes.asset";
        public const string ResourcePath = "Recipes";

        public Recipe[] Recipes = new Recipe[0];


        public void Add(Recipe recipe)
        {
            var newRecipes = new Recipe[Recipes.Length + 1];
            for (int i = 0; i < Recipes.Length; i++)
            {
                newRecipes[i] = Recipes[i];
            }
            Recipes = newRecipes;
            Recipes[^1] = recipe;
        }

        public void Remove(Recipe recipe)
        {
            var newRecipes = new Recipe[Recipes.Length - 1];
            int j = 0;
            for (int i = 0; i < Recipes.Length; i++)
            {
                if (Recipes[i] != recipe)
                {
                    newRecipes[j] = Recipes[i];
                    j++;
                }
            }
            Recipes = newRecipes;
        }

        public void Sort()
        {
            Array.Sort(Recipes, (x, y) =>
            {
                if(x.CanCraft() && !y.CanCraft())
                {
                    return -1;
                }
                if(!x.CanCraft() && y.CanCraft())
                {
                    return 1;
                }
                return x.Name.CompareTo(y.Name);
            });
        }
    }
}