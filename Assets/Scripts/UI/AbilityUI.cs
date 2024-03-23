using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using Character.Abilities;
using UnityEngine.EventSystems;
using Utility.Events;

public class AbilityUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Ability")]
    private string abilityName;
    private PlayerCharacter player;
    private float cooldown;
    [SerializeField] private AbilitySlots abilitySlot;
    [SerializeField] private AbilityDefintion ability;
    [SerializeField] private Image abilityImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite abilitySprite;
    [SerializeField] private bool isCoolDown;
    
    
    void Start()
    {
        abilityImage.fillAmount = 1;
        player = PlayerManager.Instance.GetPlayerCharacter();
        player.playerAbilitiesHandler.AddListener(abilitySlot, SetAbility);
        ability = player.playerAbilitiesHandler.GetAbility(abilitySlot);
        SetUpAbilityUI();
    }

    void Update()
    {
        if(ability == null) return;

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
        if (ability == null) return;
        abilityName = ability.DisplayName;
        cooldown = ability.Cooldown;
        if (ability.Icon != null)
            abilitySprite = ability.Icon;
        abilityImage.sprite = abilitySprite;
        backgroundImage.sprite = abilitySprite;
    }
    public void SetAbility(AbilityDefintion newAbility)
    {
        ability = newAbility;
        SetUpAbilityUI();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Defer("TooltipExited", new EventData { source = gameObject }, Utility.Collections.Priority.VeryLow);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var tooltipEvent = new TooltipEvent
        {
            Title = abilityName,
            Icon = abilitySprite,
            Description = ability.name,
            source = gameObject
        };
        EventManager.Defer("TooltipEntered", tooltipEvent, Utility.Collections.Priority.VeryLow);
    }
}
