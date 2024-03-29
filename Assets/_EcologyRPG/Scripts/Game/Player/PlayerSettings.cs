using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Items;
using EcologyRPG.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EcologyRPG.Game.Player
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("References")]
        public InputActionReference Movement;
        public InputActionReference Interact;
        public InputActionReference Sprint;
        public InputActionReference Dodge;
        public InputActionReference WeaponAttack;
        public InputActionReference Ability1;
        public InputActionReference Ability2;
        public InputActionReference Ability3;
        public InputActionReference Ability4;

        [Header("Movement Settings")]
        public float SprintMultiplier = 1.2f;

        [Header("Ability Settings")]
        public float GlobalCooldown = 0.5f;
        public PlayerAbilityDefinition SprintAbility;
        public PlayerAbilityDefinition DodgeAbility;
        public PlayerAbilityDefinition FistAttackAbility;

        [Header("Inventory Settings")]
        public Item[] StartingItems = new Item[5];

        [Header("Level Settings")]
        public List<float> XpRequiredPerLevel = new List<float>(10);

        [Header("Ohter")]
        [CharacterTag]
        public List<string> Tags = new List<string>();
    }
}
