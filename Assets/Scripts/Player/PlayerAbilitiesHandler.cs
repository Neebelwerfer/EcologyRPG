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
    Sprint Test;

    PlayerCharacter Player;

    public void Initialize(PlayerCharacter player)
    {
        Test = new Sprint();
        Player = player;
        settings = player.playerSettings;
        //settings.Dodge.action.Enable();
        //settings.Ability1.action.Enable();
        //settings.Ability2.action.Enable();
        //settings.Ability3.action.Enable();
        //settings.Ability4.action.Enable();
        Player.playerSettings.Sprint.action.Enable();
        settings.Sprint.action.started += (ctx) => {
            Debug.Log("Sprint started");
            Test.Activate(new CasterInfo { owner = Player, activationInput = settings.Sprint });
            };

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
