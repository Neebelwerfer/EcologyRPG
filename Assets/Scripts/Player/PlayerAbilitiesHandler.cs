using Character.Abilities;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilitiesHandler : IPlayerModule
{
    PlayerSettings settings;


    BaseAbility[] abilitySlots = new BaseAbility[4];
    BaseAbility dodgeAbility;
    BaseAbility sprintAbility;

    PlayerCharacter Player;

    public void Initialize(PlayerCharacter player)
    {
        Player = player;
        settings = player.playerSettings;
        settings.Sprint.action.Enable();
        settings.Dodge.action.Enable();
        settings.Ability1.action.Enable();
        settings.Ability2.action.Enable();
        settings.Ability3.action.Enable();
        settings.Ability4.action.Enable();

        sprintAbility = settings.SprintAbility;
        dodgeAbility = settings.DodgeAbility;

        settings.Sprint.action.performed += ctx => sprintAbility.Activate(new CasterInfo { activationInput = settings.Sprint, castPos = player.transform.position, owner = Player });

    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdate()
    {
    }

    public void LateUpdate()
    {
    }

}
