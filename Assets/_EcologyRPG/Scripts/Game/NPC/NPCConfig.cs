using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EcologyRPG._Game.NPC
{
    [CreateAssetMenu(fileName = "NPCConfig", menuName = "NPC/Config")]
    public class NPCConfig : ScriptableObject
    {
        [CharacterTag]
        public List<string> tags;
        public NPCBehaviour NPCBehaviour;
        public uint Level;
        public uint xp;
    }
}