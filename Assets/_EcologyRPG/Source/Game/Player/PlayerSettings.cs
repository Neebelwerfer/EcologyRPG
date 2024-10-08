using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Items;
using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EcologyRPG.GameSystems.PlayerSystems
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Model & Camera")]
        public GameObject PlayerModel;
        public GameObject Camera;

        [Header("References")]
        public InputActionReference Movement;
        public InputActionReference Interact;

        [Header("Movement Settings")]
        public float SprintMultiplier = 1.2f;
        public float rotationSpeed = 4f;

        [Header("Sound Settings")]
        public AudioClip Walking;

        [Header("Ability Settings")]
        public float GlobalCooldown = 0.5f;
        public PlayerAbilityDefinition SprintAbility;
        public PlayerAbilityDefinition DodgeAbility;
        public PlayerAbilityDefinition FistAttackAbility;

        [Header("Inventory Settings")]
        public Item[] StartingItems = new Item[5];

        [Header("Level Settings")]
        public List<float> XpRequiredPerLevel = new List<float>(10);

        [Header("Other")]
        [CharacterTag]
        public List<string> Tags = new List<string>();
    }
}
