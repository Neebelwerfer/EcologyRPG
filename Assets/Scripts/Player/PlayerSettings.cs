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

        [Header("Sprint & Dodge Definitions")]
        public BaseAbility SprintAbility;
        public BaseAbility DodgeAbility;
    }
}
