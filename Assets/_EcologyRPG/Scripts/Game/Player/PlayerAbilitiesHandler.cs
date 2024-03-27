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
        readonly InputActionReference[] abilityInputs = new InputActionReference[7];

        PlayerCharacter Player;

        Action<InputAction.CallbackContext> sprintAction;
        Action<InputAction.CallbackContext> dodgeAction;
        Action<InputAction.CallbackContext> WeaponAttackAction;
        Action<InputAction.CallbackContext> Ability1Action;
        Action<InputAction.CallbackContext> Ability2Action;
        Action<InputAction.CallbackContext> Ability3Action;
        Action<InputAction.CallbackContext> Ability4Action;

        public UnityEvent<AbilityDefintion>[] OnAbilityChange = new UnityEvent<AbilityDefintion>[7];

        int ActiveSlot = -1;

        public override void Initialize(PlayerCharacter player)
        {
            Player = player;
            settings = player.playerSettings;
            settings.Sprint.action.Enable();
            settings.Dodge.action.Enable();
            settings.WeaponAttack.action.Enable();
            settings.Ability1.action.Enable();
            settings.Ability2.action.Enable();
            settings.Ability3.action.Enable();
            settings.Ability4.action.Enable();

            abilityInputs[0] = settings.Ability1;
            Ability1Action = (ctx) => ActivateAbility(0, ctx);
            abilityInputs[1] = settings.Ability2;
            Ability2Action = (ctx) => ActivateAbility(1, ctx);
            abilityInputs[2] = settings.Ability3;
            Ability3Action = (ctx) => ActivateAbility(2, ctx);
            abilityInputs[3] = settings.Ability4;
            Ability4Action = (ctx) => ActivateAbility(3, ctx);

            if (player.Inventory.equipment.GetEquipment(EquipmentType.Weapon) is Weapon weapon)
            {
                abilitySlots[4] = Init(weapon.WeaponAbility);
            }
            else
            {
                abilitySlots[4] = Init(settings.FistAttackAbility);
            }
            abilityInputs[4] = settings.WeaponAttack;
            WeaponAttackAction = (ctx) => ActivateAbility(4, ctx);


            abilitySlots[5] = Init(settings.DodgeAbility);
            abilityInputs[5] = settings.Dodge;
            dodgeAction = (ctx) => ActivateAbility(5, ctx);
            abilitySlots[6] = Init(settings.SprintAbility);
            abilityInputs[6] = settings.Sprint;
            sprintAction = (ctx) => ActivateAbility(6, ctx);

            settings.WeaponAttack.action.started += WeaponAttackAction;
            settings.Sprint.action.started += sprintAction;
            settings.Dodge.action.started += dodgeAction;
            settings.Ability1.action.started += Ability1Action;
            settings.Ability2.action.started += Ability2Action;
            settings.Ability3.action.started += Ability3Action;
            settings.Ability4.action.started += Ability4Action;

            player.Inventory.equipment.EquipmentUpdated.AddListener(OnEquipmentChange);

        }

        public override void Update()
        {
            base.Update();
            if (ActiveSlot > -1)
            {
                if (abilityInputs[ActiveSlot].action.IsPressed())
                {
                    var ability = abilitySlots[ActiveSlot];
                    if (ability == null)
                    {
                        ActiveSlot = -1;
                        return;
                    }
                    var castInfo = new CastInfo { activationInput = abilityInputs[ActiveSlot].action, castPos = Player.AbilityPoint.transform.position, owner = Player };
                    ability.Activate(castInfo);
                }
            }
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

        void ActivateAbility(int slot, InputAction.CallbackContext context)
        {
            var ability = abilitySlots[slot];
            if (ability == null) return;

            ActiveSlot = slot;
            if (ability.Activate(new CastInfo { activationInput = context.action, castPos = Player.AbilityPoint.transform.position, owner = Player }))
            {

            }
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
            settings.Sprint.action.started -= sprintAction;
            settings.Dodge.action.started -= dodgeAction;
            settings.WeaponAttack.action.started -= WeaponAttackAction;
            settings.Ability1.action.started -= Ability1Action;
            settings.Ability2.action.started -= Ability2Action;
            settings.Ability3.action.started -= Ability3Action;
            settings.Ability4.action.started -= Ability4Action;
        }
    }
}

