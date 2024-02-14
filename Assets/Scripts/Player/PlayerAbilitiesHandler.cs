using Character.Abilities;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerAbilitiesHandler : IPlayerModule
{
    PlayerSettings settings;

    


    BaseAbility[] abilitySlots = new BaseAbility[7];

    PlayerCharacter Player;

    Action<InputAction.CallbackContext> sprintAction;
    Action<InputAction.CallbackContext> dodgeAction;


    public void Initialize(PlayerCharacter player)
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


        abilitySlots[5] = settings.DodgeAbility;
        dodgeAction = (ctx) => ActivateAbility(5);
        abilitySlots[6] = settings.SprintAbility;
        sprintAction = (ctx) => ActivateAbility(6);


        settings.Sprint.action.started += sprintAction;
        settings.Dodge.action.started += dodgeAction;

    }

    void ActivateAbility(int slot)
    {
        var ability = abilitySlots[slot];
        if (ability == null) return;
        if(ability.Activate(new CasterInfo { activationInput = settings.Ability1, castPos = Player.transform.position, owner = Player }))
        {

        }
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {
    }

    public void LateUpdate()
    {
    }

    public void OnDestroy()
    {
        settings.Sprint.action.started -= sprintAction;
        settings.Dodge.action.started -= dodgeAction;
    }
}
