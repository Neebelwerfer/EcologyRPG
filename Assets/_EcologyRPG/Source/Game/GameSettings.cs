using EcologyRPG.GameSystems.PlayerSystems;
using UnityEngine;

namespace EcologyRPG.GameSystems
{
    public class GameSettings : ScriptableObject
    {
        public PlayerSettings playerSettings;

        [Header("Layer settings")]
        public LayerMask EntityMask;
        public LayerMask TargetGroundMask;
        public LayerMask WalkableGroundMask;

        [Header("Character settings")]
        public float BaseMoveSpeed = 5f;
    }
}