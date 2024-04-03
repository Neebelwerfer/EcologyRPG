using EcologyRPG.Core.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using EcologyRPG.Core.Items;
using System.Collections;

namespace EcologyRPG.GameSystems.UI
{
    public class EquipmentButton : Button, ITooltip
    {
        public TextMeshProUGUI Name;
        public Image Image;
        public Action Unequip;

        Item item;


        bool clickedOnce = false;


        public void Setup(Item item)
        {
            if (Name == null)
                Name = transform.parent.Find("Name").GetComponent<TextMeshProUGUI>();
            if (Image == null)
                Image = GetComponent<Image>();
            if (item == null)
            {
                Name.text = "Empty";
                Image.sprite = null;
            }
            else
            {
                this.item = item;
                Name.text = item.Name;
                Image.sprite = item.Icon;
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (item == null) return;
            Tooltip.ShowTooltip(this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.HideTooltip(this);
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
            if (item == null) return;
            if (eventData.button == PointerEventData.InputButton.Left)
            {


                if (!clickedOnce)
                {
                    clickedOnce = true;
                    StartCoroutine(ResetClick());
                    return;
                }
                if (clickedOnce)
                {
                    Unequip?.Invoke();
                    clickedOnce = false;
                    return;
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                ContextUI.Instance.Open();
                ContextUI.Instance.CreateButton("Unequip", Unequip);
            }
        }


        protected override void OnDisable()
        {
            base.OnDisable();
            Tooltip.HideTooltip(gameObject);
        }

        public IEnumerator ResetClick()
        {
            yield return new WaitForSeconds(0.5f);
            clickedOnce = false;
        }

        public TooltipData GetTooltipData()
        {
            return new TooltipData() { Title = item.Name, Icon = item.Icon, Description = item.GetDisplayString() };
        }
    }
}