using EcologyRPG.GameSystems.PlayerSystems;
using UnityEngine;

namespace EcologyRPG.GameSystems
{
    public class GameSettings : ScriptableObject
    {
        public PlayerSettings playerSettings;

        [Header("Layer settings")]
        public LayerMask EntityMask;
        public LayerMask GroundMask;
        public LayerMask lootGroundLayer;

        [Header("Character settings")]
        public float BaseMoveSpeed = 5f;
    }
}