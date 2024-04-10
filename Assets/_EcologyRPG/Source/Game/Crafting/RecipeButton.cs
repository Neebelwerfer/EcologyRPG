using EcologyRPG.Core.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EcologyRPG.GameSystems.Crafting
{
    public class RecipeButton : Button, ITooltip
    {
        Recipe recipe;

        bool wasClicked;

        public void Setup(Recipe recipe)
        {
            this.recipe = recipe;
            GetComponentInChildren<TextMeshProUGUI>().text = recipe.Name;
            if(!recipe.CanCraft())
            {
                interactable = false;
            }
        }

        public TooltipData GetTooltipData()
        {
            return new TooltipData
            {
                Title = recipe.Name,
                Description = recipe.Description + "\n" + recipe.GetRequiredItemsString(),
            };
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            Tooltip.ShowTooltip(this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Tooltip.HideTooltip(this);
        }

        void Craft()
        {
            recipe.Craft();
            if(recipe.OneTimeCraft)
            {
                Destroy(gameObject);
                CraftingUI.Instance.blockedCrafts.Add(recipe.Name);
            }
            if(!recipe.CanCraft())
            {
                interactable = false;
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!interactable) return;
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (!wasClicked)
                {
                    Craft();
                    StartCoroutine(resetClick());
                }
                else
                {
                    StopAllCoroutines();
                    wasClicked = false;
                }
            }
            else
            {
                ContextUI.Instance.Open();
                ContextUI.Instance.CreateButton("Craft", () => Craft());
            }

        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Tooltip.HideTooltip(this);
        }

        IEnumerator resetClick()
        {
            yield return new WaitForSeconds(0.5f);
            wasClicked = false;
        }
    }
}