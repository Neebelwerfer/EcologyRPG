using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Character;
using System.Collections;
using UnityEngine;

namespace EcologyRPG.GameSystems.PlayerSystems
{
    [CreateAssetMenu(fileName = "New Player Ability Group", menuName = "Ability/Player Ability Group")]
    public class PlayerAbilityGroup : PlayerAbilityDefinition
    {
        [SerializeField] PlayerAbilityDefinition[] abilities;

        short currentIndex = 0;
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (abilities.Length != 0)
            {
                Setup(abilities[0]);
            }
        }
#endif

        public override void Initialize(BaseCharacter owner, AbilityDefintion prefabAbility)
        {
            currentIndex = 0;
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i] = Instantiate(abilities[i]);
                abilities[i].Initialize(owner, abilities[i]);
            }
            base.Initialize(owner, prefabAbility);
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

        public override bool CanActivate(BaseCharacter caster)
        {
            if(state != AbilityStates.ready) return false;
            return abilities[currentIndex].CanActivate(caster);
        }


        public override void PutOnCooldown()
        {
            base.PutOnCooldown();
            currentIndex = (short)((currentIndex + 1) % abilities.Length);
            Setup(abilities[currentIndex]);
        }
    }
}
