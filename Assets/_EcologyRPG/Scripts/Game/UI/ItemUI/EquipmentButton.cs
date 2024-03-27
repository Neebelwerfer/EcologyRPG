using EcologyRPG.Core.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System;
using EcologyRPG.Core.Items;
using System.Collections;

namespace EcologyRPG.Game.UI
{
    public class EquipmentButton : Button
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
            Tooltip.ShowTooltip(gameObject, new TooltipData() { Title = item.Name, Icon = item.Icon, Description = item.GetDisplayString() });
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.HideTooltip(gameObject);
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
    }
}