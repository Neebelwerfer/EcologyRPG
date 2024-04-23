using EcologyRPG.AbilityScripting;
using UnityEngine;

namespace EcologyRPG.Core.Abilities
{
    public class VFXObjectDatabase : ScriptableObject
    {
        public static VFXObjectDatabase Instance;
        public const string ResourcePath = "VFXObjectDatabase";
        public const string ResourceFullPath = "Assets/_EcologyRPG/Resources/" + ResourcePath + ".asset";

        public GameObject[] vfxObjects = new GameObject[0];


        public static void Load()
        {
            Instance = Resources.Load<VFXObjectDatabase>(ResourcePath);
        }
    }
}