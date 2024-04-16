using EcologyRPG.Core.Abilities.AbilityData;
using EcologyRPG.Core.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using EcologyRPG.Core.Abilities;

namespace EcologyRPG.GameSystems.UI
{
    public class AbilityButton : Button, ITooltip
    {
        PlayerAbilityDefinition ability;

        public void Setup(PlayerAbilityDefinition ability)
        {
            this.ability = ability;
            ability.AbilityChanged.AddListener(OnAbilityChanged);
            image.sprite = ability.Icon;
        }

        private void OnAbilityChanged()
        {
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

        protected override void OnEnable()
        {
            if (ability == null) return;
            image.sprite = ability.Icon;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Tooltip.HideTooltip(this);
        }

        public TooltipData GetTooltipData()
        {
            if(AbilityManager.UseToxic && ability.ToxicAbility != null)
            {
                return new TooltipData() { Title = ability.DisplayName, Subtitle = "Toxic Ability", Icon = ability.Icon, Description = ability.GetDescription() };

            }
            return new TooltipData() { Title = ability.DisplayName, Subtitle = "Normal Ability", Icon = ability.Icon, Description = ability.GetDescription() };
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (ability != null) ability.AbilityChanged.RemoveListener(OnAbilityChanged);
        }
    }
}