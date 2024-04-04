using EcologyRPG.GameSystems.PlayerSystems;
using UnityEngine;

namespace EcologyRPG.GameSystems
{
    public class GameSettings : ScriptableObject
    {
        public PlayerSettings playerSettings;

        public LayerMask EntityMask;
        public LayerMask GroundMask;
        public LayerMask lootGroundLayer;
    }
}