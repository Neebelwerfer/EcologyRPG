using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Character;
using EcologyRPG.GameSystems.PlayerSystems;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace EcologyRPG.GameSystems.PlayerSystems
{
    [CreateAssetMenu(fileName = "New Player Ability Group", menuName = "Ability/Player Ability Group")]
    public class PlayerAbilityGroup : PlayerAbilityDefinition
    {
        [SerializeField] PlayerAbilityDefinition[] abilities;

        short currentIndex = 0;

        private void OnValidate()
        {
            if (abilities.Length != 0)
            {
                Setup(abilities[0]);
            }
        }

        public override void Initialize(BaseCharacter owner)
        {
            currentIndex = 0;
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i] = Instantiate(abilities[i]);
                abilities[i].Initialize(owner);
            }
            base.Initialize(owner);
            Setup(abilities[currentIndex]);
            SetupData(abilities[currentIndex]);
        }

        void Setup(PlayerAbilityDefinition abilityData)
        {
            Ability = abilityData.Ability;
            Icon = abilityData.Icon;
            DisplayName = abilityData.DisplayName;
            Description = abilityData.Description;
            ToxicAbility = abilityData.ToxicAbility;
            AbilityChanged.Invoke();
        }

        void SetupData(PlayerAbilityDefinition abilityData)
        {
            ToxicResoureCost = abilityData.ToxicResoureCost;
            ToxicSelfDamage = abilityData.ToxicSelfDamage;
            BlockMovementOnWindup = abilityData.BlockMovementOnWindup;
            ReducedSpeedOnWindup = abilityData.ReducedSpeedOnWindup;
            BlockRotationOnWindup = abilityData.BlockRotationOnWindup;
            UseMouseDirection = abilityData.UseMouseDirection;
            RotatePlayerTowardsMouse = abilityData.RotatePlayerTowardsMouse;
            TriggerHash = abilityData.TriggerHash;
            ResourceName = abilityData.ResourceName;
            ResourceCost = abilityData.ResourceCost;
            Cooldown = abilityData.Cooldown;
            CastWindUp = abilityData.CastWindUp;
            CastWindupTime = abilityData.CastWindupTime;
        }

        public override void InitialCastCost(CastInfo caster)
        {
            abilities[currentIndex].InitialCastCost(caster);
        }

        protected override void OnReady()
        {
            base.OnReady();
            SetupData(abilities[currentIndex]);
        }


        public override void PutOnCooldown()
        {
            base.PutOnCooldown();
            currentIndex = (short)((currentIndex + 1) % abilities.Length);
            Setup(abilities[currentIndex]);
        }
    }
}

[CustomEditor(typeof(PlayerAbilityGroup))]
public class PlayerAbilityGroupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilities"), true);
        serializedObject.ApplyModifiedProperties();
    }
}