using System;
using UnityEngine;

namespace EcologyRPG.Core.Items
{
    [Serializable]
    public class GenerationRules : ScriptableObject
    {
        [LootCategory]
        public string DropCategory = "";
        public int DropChanceWeight = 10;
        public string[] allowedTags = new string[0];
    }
}