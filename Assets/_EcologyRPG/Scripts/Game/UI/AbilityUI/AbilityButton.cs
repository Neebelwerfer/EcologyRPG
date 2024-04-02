using EcologyRPG._Core.Abilities.AbilityData;
using EcologyRPG._Core.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace EcologyRPG._Game.UI
{
    public class AbilityButton : Button, ITooltip
    {
        PlayerAbilityDefinition ability;

        public void Setup(PlayerAbilityDefinition ability)
        {
            this.ability = ability;
            image.sprite = ability.Icon;
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.HideTooltip(gameObject);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (ability == null) return;
            Tooltip.ShowTooltip(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Tooltip.HideTooltip(this);
        }

        public TooltipData GetTooltipData()
        {
            return new TooltipData() { Title = ability.DisplayName, Icon = ability.Icon, Description = ability.Description };
        }
    }
}