using EcologyRPG.Core.Character;
using EcologyRPG.GameSystems.PlayerSystems;
using UnityEngine;

namespace EcologyRPG.GameSystems
{
    public class GameSettings : ScriptableObject
    {
        public const string GroundHazardName = "GroundHazard";

        public PlayerSettings playerSettings;

        [Header("Layer settings")]
        public LayerMask EntityMask;
        public LayerMask TargetGroundMask;
        public LayerMask WalkableGroundMask;

        [Header("Character settings")]
        public float BaseMoveSpeed = 5f;

        [Header("NPC settings")] 
        public float NPCDeadBodyDespawnTime = 3f;

        [Header("Ability settings")]
        [StatAttribute(StatType.Resource)]
        public string ToxicResourceName;
        public Color ToxicAbilityReady = Color.white;
        public Color ToxicAbilityNotReady = Color.gray;
    }
}