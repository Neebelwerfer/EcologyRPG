using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Items;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EcologyRPG.Game.Player
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

    public class PlayerAbilitiesHandler : PlayerModule
    {
        PlayerSettings settings;
        readonly PlayerAbilityDefinition[] abilitySlots = new PlayerAbilityDefinition[7];

        PlayerCharacter Player;

        public UnityEvent<AbilityDefintion>[] OnAbilityChange = new UnityEvent<AbilityDefintion>[7];

        public override void Initialize(PlayerCharacter player)
        {
            Player = player;
            settings = player.playerSettings;

            if (player.Inventory.equipment.GetEquipment(EquipmentType.Weapon) is Weapon weapon)
            {
                abilitySlots[4] = Init(weapon.WeaponAbility);
            }
            else
            {
                abilitySlots[4] = Init(settings.FistAttackAbility);
            }


            abilitySlots[5] = Init(settings.DodgeAbility);
            abilitySlots[6] = Init(settings.SprintAbility);

            player.Inventory.equipment.EquipmentUpdated.AddListener(OnEquipmentChange);

        }

        public override void Update()
        {
            base.Update();
                //if (abilityInputs[ActiveSlot].action.IsPressed())
                //{
                //    var ability = abilitySlots[ActiveSlot];
                //    if (ability == null)
                //    {
                //        ActiveSlot = -1;
                //        return;
                //    }
                //    var castInfo = new CastInfo { activationInput = abilityInputs[ActiveSlot].action, castPos = Player.CastPos, owner = Player };
                //    ability.Activate(castInfo);
                //}
        }

        PlayerAbilityDefinition Init(PlayerAbilityDefinition ability)
        {
            var newAbility = UnityEngine.Object.Instantiate(ability);
            newAbility.Initialize(Player);
            return newAbility;
        }

        private void OnEquipmentChange(int arg0)
        {
            if (arg0 == (int)EquipmentType.Weapon)
            {
                var item = Player.Inventory.equipment.GetEquipment(EquipmentType.Weapon);
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
                if (abilitySlots[i].DisplayName == ability.DisplayName)
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

        public override void OnDestroy()
        {

        }
    }
}

