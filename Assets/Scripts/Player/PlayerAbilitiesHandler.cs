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


    BaseAbility[] abilitySlots = new BaseAbility[4];
    BaseAbility dodgeAbility;
    BaseAbility sprintAbility;

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

        sprintAbility = settings.SprintAbility;
        dodgeAbility = settings.DodgeAbility;

        sprintAction = (ctx) => ActivateSprint();
        dodgeAction = (ctx) => ActivateDodge();

        settings.Sprint.action.started += sprintAction;
        settings.Dodge.action.started += dodgeAction;

    }

    void ActivateSprint()
    {
        sprintAbility.Activate(new CasterInfo { activationInput = settings.Sprint, castPos = Player.transform.position, owner = Player });
    }

    void ActivateDodge()
    {
        dodgeAbility.Activate(new CasterInfo { activationInput = settings.Dodge, castPos = Player.transform.position, owner = Player });
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
