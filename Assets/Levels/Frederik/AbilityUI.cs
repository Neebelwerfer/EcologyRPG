using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using Character.Abilities;

public class AbilityUI : MonoBehaviour
{
    [Header("Ability")]
    private string abilityName;
    private PlayerCharacter Player;
    [SerializeField] private Image abilityImage;
    [SerializeField] private float cooldown;
    [SerializeField] private bool isCoolDown;
    
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerCharacter>();
        abilityImage.fillAmount = 1;
        abilityName = Player.playerSettings.WeaponAttackAbility.name;
        cooldown = Player.playerSettings.WeaponAttackAbility.Cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldown();
    }
    private void UpdateCooldown()
    {
        if (Player.playerSettings.WeaponAttackAbility.state.Equals(AbilityStates.ready))
        {
            abilityImage.fillAmount = 1;
        }
        else if (Player.playerSettings.WeaponAttackAbility.state.Equals(AbilityStates.cooldown))
        {
            abilityImage.fillAmount = 1 - (Player.playerSettings.WeaponAttackAbility.remainingCooldown)/cooldown;
        }
    }
}
