using System;
using UnityEngine;

namespace EcologyRPG.GameSystems.Crafting
{
    [Serializable, CreateAssetMenu(fileName = "Quest Recipe", menuName = "Recipe/Quest Recipe")]
    public class QuestRecipe : Recipe
    {
        public string FlagName;
        public int FlagValue;
    }
}