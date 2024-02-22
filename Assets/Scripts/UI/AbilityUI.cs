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
    private PlayerCharacter player;
    private float cooldown;
    [SerializeField] private BaseAbility ability;
    [SerializeField] private Image abilityImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite abilitySprite;
    [SerializeField] private bool isCoolDown;
    
    void Start()
    {
        abilityImage.fillAmount = 1;
        SetUpAbilityUI();
    }

    void Update()
    {
        UpdateCooldown();
    }
    private void UpdateCooldown()
    {
        if (ability.state.Equals(AbilityStates.ready))
        {
            abilityImage.fillAmount = 1;
        }
        else if (ability.state.Equals(AbilityStates.cooldown))
        {
            abilityImage.fillAmount = 1 - (ability.remainingCooldown)/cooldown;
        }
    }
    public void SetUpAbilityUI()
    {
        abilityName = ability.name;
        cooldown = ability.Cooldown;
        abilityImage.sprite = abilitySprite;
        backgroundImage.sprite = abilitySprite;
    }
    public void SetAbility(BaseAbility newAbility)
    {
        ability = newAbility;
        //TODO: GET SPRITE FROM ABILITY
        SetUpAbilityUI();
    }
}
