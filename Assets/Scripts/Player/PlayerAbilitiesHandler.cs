using Character.Abilities;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Utility;

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
    readonly BaseAbility[] abilitySlots = new BaseAbility[7];

    PlayerCharacter Player;

    Action<InputAction.CallbackContext> sprintAction;
    Action<InputAction.CallbackContext> dodgeAction;
    Action<InputAction.CallbackContext> WeaponAttackAction;

    public UnityEvent<BaseAbility>[] OnAbilityChange = new UnityEvent<BaseAbility>[7];

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

        abilitySlots[4] = settings.WeaponAttackAbility;
        WeaponAttackAction = (ctx) => ActivateAbility(4, ctx);
        abilitySlots[5] = settings.DodgeAbility;
        dodgeAction = (ctx) => ActivateAbility(5, ctx);
        abilitySlots[6] = settings.SprintAbility;
        sprintAction = (ctx) => ActivateAbility(6, ctx);

        settings.WeaponAttack.action.started += WeaponAttackAction;
        settings.Sprint.action.started += sprintAction;
        settings.Dodge.action.started += dodgeAction;
    }

    public BaseAbility GetAbility(AbilitySlots slot)
    {
        return abilitySlots[(int)slot];
    }

    void ActivateAbility(int slot, InputAction.CallbackContext context)
    {
        var ability = abilitySlots[slot];
        if (ability == null) return;
        if(ability.Activate(new CasterInfo { activationInput = context.action, castPos = Player.AbilityPoint.transform.position, owner = Player }))
        {

        }
    }

    public void AddListener(AbilitySlots slot, UnityAction<BaseAbility> action)
    {
        if (OnAbilityChange[(int)slot] == null)
        {
            OnAbilityChange[(int)slot] = new UnityEvent<BaseAbility>();
        }
        OnAbilityChange[(int)slot].AddListener(action);
    }

    public override void OnDestroy()
    {
        settings.Sprint.action.started -= sprintAction;
        settings.Dodge.action.started -= dodgeAction;
        settings.WeaponAttack.action.started -= WeaponAttackAction;
    }
}
