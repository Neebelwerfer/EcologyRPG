using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG.Utility
{
    public class Tags : ScriptableObject
    {
        public const string path = "Assets/_EcologyRPG/Resources/Config/CharacterTags.asset";
        public List<string> tags = new List<string>();

    }


    public class CharacterTag : PropertyAttribute
    {

    }
}