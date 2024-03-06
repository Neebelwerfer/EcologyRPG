using Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/PlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("References")]
        public InputActionReference Movement;
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
        public BaseAbility SprintAbility;
        public BaseAbility DodgeAbility;
        public BaseAbility Ability1Reference;
        public BaseAbility Ability2Reference;
        public BaseAbility Ability3Reference;
        public BaseAbility Ability4Reference;
        public BaseAbility WeaponAttackAbility;

        [Header("Inventory Settings")]
        public Item[] StartingItems = new Item[5];

        [Header("Level Settings")]
        public List<float> XpRequiredPerLevel = new List<float>(10);
    }
}
