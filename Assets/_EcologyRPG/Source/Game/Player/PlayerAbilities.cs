using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Items;
using System;
using UnityEngine.Events;
using UnityEngine;

namespace EcologyRPG.GameSystems.PlayerSystems
{
    public enum AbilitySlots
    {
        Ability1,
        Ability2,
        Ability3,
        Ability4,
        WeaponAttack,
        Dodge,
        Sprint
    }

    public class PlayerAbilities
    {
        readonly PlayerSettings settings;
        readonly PlayerCharacter _Player;
        readonly PlayerAbilityDefinition[] abilitySlots = new PlayerAbilityDefinition[7];


        public UnityEvent<AbilityDefintion>[] OnAbilityChange = new UnityEvent<AbilityDefintion>[7];

        public PlayerAbilities(PlayerCharacter player, Inventory inventory, PlayerSettings settings)
        {
            _Player = player;
            this.settings = settings;

            abilitySlots[4] = Init(settings.FistAttackAbility);
            abilitySlots[5] = Init(settings.DodgeAbility);
            abilitySlots[6] = Init(settings.SprintAbility);

            inventory.equipment.EquipmentUpdated.AddListener(OnEquipmentChange);
        }

        PlayerAbilityDefinition Init(PlayerAbilityDefinition ability)
        {
            var newAbility = UnityEngine.Object.Instantiate(ability);
            newAbility.Initialize(_Player, ability);
            return newAbility;
        }

        private void OnEquipmentChange(int arg0)
        {
            if (arg0 == (int)EquipmentType.Weapon)
            {
                var item = Player.PlayerInventory.equipment.GetEquipment(EquipmentType.Weapon);
                if (item == null || ((Weapon)item).WeaponAbility == null) SetAbility(AbilitySlots.WeaponAttack, settings.FistAttackAbility);
                else if (item is Weapon weapon) SetAbility(AbilitySlots.WeaponAttack, weapon.WeaponAbility);
            }
        }

        public AbilityDefintion GetAbility(AbilitySlots slot)
        {
            return abilitySlots[(int)slot];
        }

        public bool GotAbility(PlayerAbilityDefinition ability, out AbilitySlots? slot)
        {
            for (int i = 0; i < abilitySlots.Length; i++)
            {
                if (abilitySlots[i] == null) continue;
                if (abilitySlots[i].GUID == ability.GUID)
                {
                    slot = (AbilitySlots)i;
                    return true;
                }
            }
            slot = null;
            return false;
        }

        public void SetAbility(AbilitySlots slot, PlayerAbilityDefinition ability)
        {
            if (ability == null)
                abilitySlots[(int)slot] = null;
            else
                abilitySlots[(int)slot] = Init(ability);

            OnAbilityChange[(int)slot]?.Invoke(abilitySlots[(int)slot]);
        }

        public void AddListener(AbilitySlots slot, UnityAction<AbilityDefintion> action)
        {
            if (OnAbilityChange[(int)slot] == null)
            {
                OnAbilityChange[(int)slot] = new UnityEvent<AbilityDefintion>();
            }
            OnAbilityChange[(int)slot].AddListener(action);
        }

        public void PlayerDeath()
        {
            foreach (var ability in abilitySlots)
            {
                if (ability != null && ability.state == AbilityStates.casting)
                {
                    ability.CastCancelled(new CastInfo() { owner = _Player });
                }
            }
        }
    }
}

