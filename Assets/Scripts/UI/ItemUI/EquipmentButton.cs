using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentButton : Button
{
    public TextMeshProUGUI Name;

    Item item;

    public void Setup(Item item)
    {
        if(Name == null)
            Name = transform.parent.Find("Name").GetComponent<TextMeshProUGUI>();
        if(item == null)
        {
            Name.text = "Empty";
        }
        else
        {
            this.item = item;
            Name.text = item.Name;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null) return;
        Tooltip.ShowTooltip(gameObject, new TooltipData() { Title = item.Name, Icon = item.Icon, Description = item.GetDisplayString() });
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip(gameObject);
    }
}