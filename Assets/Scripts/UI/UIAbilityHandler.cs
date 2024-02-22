using Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Unity.VisualScripting;
using UnityEditor.Playables;

public class UIAbilityHandler : MonoBehaviour
{
    private PlayerCharacter player;
    private BaseAbility weaponAttack;
    private BaseAbility dodgeAbility;
    private BaseAbility ability1;
    private BaseAbility ability2;
    private BaseAbility ability3;
    private BaseAbility ability4;

    [SerializeField] private AbilityUI weaponAttackUI;
    [SerializeField] private AbilityUI dodgeAbilityUI;
    [SerializeField] private AbilityUI ability1UI;
    [SerializeField] private AbilityUI ability2UI;
    [SerializeField] private AbilityUI ability3UI;
    [SerializeField] private AbilityUI ability4UI;

    private void Start()
    {
        
        player = FindObjectOfType<PlayerCharacter>();
        weaponAttack = player.playerSettings.WeaponAttackAbility;
        weaponAttackUI.SetAbility(weaponAttack);
        dodgeAbility = player.playerSettings.DodgeAbility;
        dodgeAbilityUI.SetAbility(dodgeAbility);
        // this is might be better in a foreach, but that would require a list, it might not be the best since we have a set amount of abilities
        //ability1 = player.playerSettings.Ability1;
        //ability1UI.SetAbility(ability1);
        //ability2 = player.playerSettings.Ability2;
        //ability2UI.SetAbility(ability2);
        //ability3 = player.playerSettings.Ability3;
        //ability3UI.SetAbility(ability3);
        //ability4 = player.playerSettings.Ability4;
        //ability4UI.SetAbility(ability4);
    }

    public void SetAbilitySlot(AbilityUI abilitySlot, BaseAbility ability)
    {
        abilitySlot.SetAbility(ability);
    }
}
