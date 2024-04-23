using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.Items;
using UnityEngine;
using UnityEngine.Events;
 
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
        readonly PlayerAbilityReference[] abilitySlots = new PlayerAbilityReference[7];


        public UnityEvent<PlayerAbilityReference>[] OnAbilityChange = new UnityEvent<PlayerAbilityReference>[7];

        public PlayerAbilities(PlayerCharacter player, Inventory inventory, PlayerSettings settings)
        {
            _Player = player;
            this.settings = settings;

            abilitySlots[4] = Init(settings.FistAttackAbility);
            //abilitySlots[5] = Init(settings.DodgeAbility);
            //abilitySlots[6] = Init(settings.SprintAbility);

            inventory.equipment.EquipmentUpdated.AddListener(OnEquipmentChange);
        }

        private PlayerAbilityReference Init(PlayerAbilityReference ability)
        {
            if (ability == null) return null;
            var newAbility = Object.Instantiate(ability);
            newAbility.Init(_Player);
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

        public PlayerAbilityReference GetAbility(AbilitySlots slot)
        {
            return abilitySlots[(int)slot];
        }

        public bool GotAbility(PlayerAbilityReference ability, out AbilitySlots? slot)
        {
            for (int i = 0; i < abilitySlots.Length; i++)
            {
                if (abilitySlots[i] == null) continue;
                if (abilitySlots[i].AbilityID == ability.AbilityID)
                {
                    slot = (AbilitySlots)i;
                    return true;
                }
            }
            slot = null;
            return false;
        }

        public void SetAbility(AbilitySlots slot, PlayerAbilityReference ability)
        {
            if (ability == null)
                abilitySlots[(int)slot] = null;
            else
                abilitySlots[(int)slot] = Init(ability);

            OnAbilityChange[(int)slot]?.Invoke(abilitySlots[(int)slot]);
        }

        public void AddListener(AbilitySlots slot, UnityAction<PlayerAbilityReference> action)
        {
            if (OnAbilityChange[(int)slot] == null)
            {
                OnAbilityChange[(int)slot] = new UnityEvent<PlayerAbilityReference>();
            }
            OnAbilityChange[(int)slot].AddListener(action);
        }

        public void PlayerDeath()
        {
            foreach (var ability in abilitySlots)
            {
                if (ability != null && ability.State == CastState.Casting)
                {
                    ability.OnCastCancelled();
                }
            }
        }
    }
}

