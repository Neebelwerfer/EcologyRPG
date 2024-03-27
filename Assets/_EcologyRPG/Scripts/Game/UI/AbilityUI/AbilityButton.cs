using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace EcologyRPG.Game.UI
{
    public class AbilityButton : Button
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
            Tooltip.ShowTooltip(gameObject, new TooltipData() { Title = ability.DisplayName, Icon = ability.Icon, Description = ability.Description });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Tooltip.HideTooltip(gameObject);
        }
    }
}