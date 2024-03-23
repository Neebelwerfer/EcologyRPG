using Items;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : Button
{
    InventoryItem item;

    TextMeshProUGUI text;

    public void Setup(InventoryItem item)
    {
        if(text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();

        this.item = item;
        text.text = item.amount + "x " + item.item.Name;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip(gameObject, new TooltipData() { Title = item.item.Name, Icon = item.item.Icon, Description = item.item.GetDisplayString() });
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip(gameObject);
    }
}