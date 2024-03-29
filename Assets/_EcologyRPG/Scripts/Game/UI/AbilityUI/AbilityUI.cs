using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.Abilities;
using EcologyRPG.Core.UI;
using EcologyRPG.Game.Player;

namespace EcologyRPG.Game.UI
{
    public class AbilityUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ITooltip
    {
        [Header("Ability")]
        private string abilityName;
        private PlayerCharacter player;
        private float cooldown;
        public AbilitySlots abilitySlot;
        [SerializeField] private PlayerAbilityDefinition ability;
        [SerializeField] private Image abilityImage;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Sprite abilitySprite;
        [SerializeField] private bool isCoolDown;


        void Start()
        {
            abilityImage.fillAmount = 1;
            player = PlayerManager.Instance.GetPlayerCharacter();
            player.playerAbilitiesHandler.AddListener(abilitySlot, SetAbility);
            ability = (PlayerAbilityDefinition)player.playerAbilitiesHandler.GetAbility(abilitySlot);
            SetUpAbilityUI();
        }

        void Update()
        {
            if (ability == null) return;

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
                abilityImage.fillAmount = 1 - (ability.remainingCooldown) / cooldown;
            }
        }
        public void SetUpAbilityUI()
        {
            if (ability == null)
            {
                abilitySprite = null;
                abilityImage.sprite = abilitySprite;
                backgroundImage.sprite = abilitySprite;
                return;
            };

            abilityName = ability.DisplayName;
            cooldown = ability.Cooldown;
            if (ability.Icon != null)
                abilitySprite = ability.Icon;
            abilityImage.sprite = abilitySprite;
            backgroundImage.sprite = abilitySprite;
        }
        public void SetAbility(AbilityDefintion newAbility)
        {
            ability = (PlayerAbilityDefinition)newAbility;
            SetUpAbilityUI();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.HideTooltip(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (ability == null) return;
            Tooltip.ShowTooltip(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            AbilitySelectionUI.Instance.Show(this);
        }

        public TooltipData GetTooltipData()
        {
            return new TooltipData() { Title = abilityName, Icon = abilitySprite, Description = ability.Description };
        }
    }
}
